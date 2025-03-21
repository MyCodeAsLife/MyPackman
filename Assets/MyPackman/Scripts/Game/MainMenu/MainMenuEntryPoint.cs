using DI;
using Game.Gameplay;
using Game.UI;
using R3;
using UnityEngine;

namespace Game.MainMenu
{
    public class MainMenuEntryPoint : MonoBehaviour     // Похожа на GameplayEntryPoint
    {
        [SerializeField] private UIMainMenuRootBinder _mainMenuUIRootPrefab;
        private DIContainer _container;

        // Возвращает объект сингала, из которго можно только считать значение GameplayExitParams
        public Observable<MainMenuExitParams> Run(DIContainer container, MainMenuEnterParams mainMenuEnterParams)
        {
            _container = container;
            var uiScene = Instantiate(_mainMenuUIRootPrefab);
            _container.Resolve<UIRootView>().AttachSceneUI(uiScene.gameObject);

            var exitSignalSubj = new Subject<Unit>();
            uiScene.Bind(exitSignalSubj);

            // Чтото делаем с полученными параметрами, вызов с вопросом, потому как параметры в MainMenu передаются не всегда
            Debug.Log($"Main Menu entry point: Run main menu scene. Results: {mainMenuEnterParams?.Result}");

            // Формируем даннные для сцены, если нет(первый запуск к примеру) то можно создать параметры по умолчанию
            var saveFileName = "Is null";
            int levelNumber = 2;
            var targetSceneEnterParams = new GameplayEnterParams(saveFileName, levelNumber);
            // Заворачиваем сформированные данные о сценене в объект выходных данных MainMenu
            var mainMenuExitParams = new MainMenuExitParams(targetSceneEnterParams);
            // Заворачиваем данные в объект сигнала с которого можно только считывать и возвращаем его
            var exitToGameplaySceneSignal = exitSignalSubj.Select(_ => mainMenuExitParams);
            return exitToGameplaySceneSignal;
        }
    }
}