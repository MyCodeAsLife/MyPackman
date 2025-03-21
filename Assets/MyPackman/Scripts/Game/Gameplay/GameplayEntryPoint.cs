using Game.MainMenu;
using Game.UI;
using DI;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameplayEntryPoint : MonoBehaviour     // Похожа на MainMenuEntryPoint
    {
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;
        private DIContainer _container;

        // Возвращает объект сингала, из которго можно только считать значение GameplayExitParams
        public Observable<GameplayExitParams> Run(DIContainer container, GameplayEnterParams sceneEnterParams)
        {
            _container = container;
            var uiScene = Instantiate(_sceneUIRootPrefab);
            _container.Resolve<UIRootView>().AttachSceneUI(uiScene.gameObject);

            var exitSceneSignalSubj = new Subject<Unit>();
            uiScene.Bind(exitSceneSignalSubj);

            // Чтото делаем с полученными параметрами, вызов с вопросом
            Debug.Log($"Gameplay entry point: Run gameplay scene. Save filename: {sceneEnterParams.SaveFileName}. " +
                      $"Scene name: {sceneEnterParams.SceneName}. Level number: {sceneEnterParams.LevelNumber}");

            // Создаем объект для возвращаемых параметров и заполняем его
            var mainMenuEnterParams = new MainMenuEnterParams("Result");
            // Заворачиваем созданный выше объект в новый объект
            var exitParams = new GameplayExitParams(mainMenuEnterParams);
            // Заворачиваем наш объект в объект сигнала с которого можно только считывать
            var exitToMainMenuSceneSignal = exitSceneSignalSubj.Select(_ => exitParams);
            // И возвращаем возвращаем его
            return exitToMainMenuSceneSignal;
        }
    }
}
