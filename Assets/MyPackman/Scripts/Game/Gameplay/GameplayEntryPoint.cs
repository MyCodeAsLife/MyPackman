using Game.MainMenu;
using Game.UI;
using DI;
using R3;
using UnityEngine;
using Game.Gameplay.Static;

namespace Game.Gameplay
{
    public class GameplayEntryPoint : MonoBehaviour     // Похожа на MainMenuEntryPoint
    {
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;
        //private DIContainer _gameplayContainer;

        // Возвращает объект сингала, из которго можно только считать значение GameplayExitParams
        public Observable<GameplayExitParams> Run(DIContainer gameplayContainer, GameplayEnterParams sceneEnterParams)
        {
            // Получаем созданный для этой сцены контейнер и кэшируем его
            //_gameplayContainer = gameplayContainer;
            // Производим регистрацию всех необходимых для данной сцены сервисов, в контейнере сцены
            GameplayRegistrations.Register(gameplayContainer, sceneEnterParams);
            // Зачем создавать второй контейнер сцены? Чтобы чтобы другие сервисы сцены немогли доставать ViewModel-и?
            var gameplayViewModelContainer = new DIContainer(gameplayContainer);    // Надо ли его кэшировать?
            GameplayViewModelRegistrations.Register(gameplayViewModelContainer);

            //for tests
            gameplayViewModelContainer.Resolve<UIGameplayRootViewModel>();
            gameplayViewModelContainer.Resolve<WorldGameplayViewModel>();

            // Создаем UI сцены из префаба и прикрепляем его к корневому UIRoot
            var uiScene = Instantiate(_sceneUIRootPrefab);
            gameplayContainer.Resolve<UIRootView>().AttachSceneUI(uiScene.gameObject);

            // Создаем объект сигнала который ничего не принимает и отдаем его UI-ю сцены, который будет его дергать
            var exitSceneSignalSubj = new Subject<Unit>();
            uiScene.Bind(exitSceneSignalSubj);

            //for tests
            Debug.Log($"Gameplay entry point: Run gameplay scene. Save filename: {sceneEnterParams.SaveFileName}. " +
                      $"Scene name: {sceneEnterParams.SceneName}. Level number: {sceneEnterParams.LevelNumber}");

            // Создаем объект для возвращаемых параметров и заполняем его
            var mainMenuEnterParams = new MainMenuEnterParams("Result");
            // Заворачиваем созданный выше объект в новый объект
            var exitParams = new GameplayExitParams(mainMenuEnterParams);
            // Заворачиваем наш объект в объект сигнала с которого можно только считывать
            var exitToMainMenuSceneSignal = exitSceneSignalSubj.Select(_ => exitParams);
            // И возвращаем его
            return exitToMainMenuSceneSignal;
        }
    }
}
