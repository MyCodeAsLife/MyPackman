using Game.Gameplay;
using Game.MainMenu;
using Game.UI;
using R3;
using UnityEngine;

namespace Assets.MyPackman.Scripts
{
    public class GameplayEntryPoint : MonoBehaviour     // Похожа на MainMenuEntryPoint
    {
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;

        // Возвращает объект сингала, из которго можно только считать значение GameplayExitParams
        public Observable<GameplayExitParams> Run(UIRootView uiRoot, GameplayEnterParams gameplayEnterParams)
        {
            var uiScene = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiScene.gameObject);

            var exitSceneSignalSubj = new Subject<Unit>();
            uiScene.Bind(exitSceneSignalSubj);

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
