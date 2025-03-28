using DI;
using Game.Gameplay.Static;
using Game.Gameplay.View;
using Game.MainMenu;
using Game.UI;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameplayEntryPoint : MonoBehaviour     // Похожа на MainMenuEntryPoint
    {
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;
        [SerializeField] private WorldGameplayRootBinder _worldRootBinder;

        // Возвращает объект сингала, из которго можно только считать значение GameplayExitParams
        public Observable<GameplayExitParams> Run(DIContainer gameplayContainer, GameplayEnterParams sceneEnterParams)
        {
            // Получаем созданный для этой сцены контейнер и кэшируем его
            //_gameplayContainer = gameplayContainer;
            // Производим регистрацию всех необходимых для данной сцены сервисов, в контейнере сцены
            GameplayRegistrations.Register(gameplayContainer, sceneEnterParams);
            // Зачем создавать второй контейнер сцены? Чтобы чтобы другие сервисы сцены немогли доставать ViewModel-и?
            var gameplayViewModelsContainer = new DIContainer(gameplayContainer);    // Надо ли его кэшировать?
            GameplayViewModelRegistrations.Register(gameplayViewModelsContainer);

            //for test
            _worldRootBinder.Bind(gameplayViewModelsContainer.Resolve<WorldGameplayRootViewModel>());
            gameplayViewModelsContainer.Resolve<UIGameplayRootViewModel>();

            // Создаем UI сцены из префаба и прикрепляем его к корневому UIRoot
            var uiScene = Instantiate(_sceneUIRootPrefab);
            gameplayContainer.Resolve<UIRootView>().AttachSceneUI(uiScene.gameObject);

            // Создаем объект сигнала который ничего не принимает и отдаем его UI-ю сцены, который будет его дергать
            var exitSceneSignalSubj = new Subject<Unit>();
            uiScene.Bind(exitSceneSignalSubj);

            //for tests
            Debug.Log($"Gameplay entry point: Run gameplay scene. " +
                      $"Scene name: {sceneEnterParams.SceneName}. " +
                      $"Map ID: {sceneEnterParams.MapId}");

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
