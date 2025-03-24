using DI;
using Game.Gameplay;
using Game.MainMenu.Static;
using Game.Services;
using Game.UI;
using R3;
using UnityEngine;

namespace Game.MainMenu
{
    public class MainMenuEntryPoint : MonoBehaviour     // Похожа на GameplayEntryPoint
    {
        [SerializeField] private UIMainMenuRootBinder _mainMenuUIRootPrefab;
        //private DIContainer _mainMenuContainer;

        // Возвращает объект сингала, из которго можно только считать значение GameplayExitParams
        public Observable<MainMenuExitParams> Run(DIContainer mainMenuContainer, MainMenuEnterParams mainMenuEnterParams)
        {
            // Получаем созданный для этой сцены контейнер и кэшируем его
            //_mainMenuContainer = mainMenuContainer;
            // Производим регистрацию всех необходимых для данной сцены сервисов, в контейнере сцены
            MainMenuRegistrations.Register(mainMenuContainer, mainMenuEnterParams);
            // Зачем создавать второй контейнер сцены? Чтобы чтобы другие сервисы сцены немогли доставать ViewModel-и?
            var mainMenuViewModelContainer = new DIContainer(mainMenuContainer);    // Надо ли его кэшировать?
            MainMenuViewModelRegistrations.Register(mainMenuViewModelContainer);

            //for tests
            mainMenuViewModelContainer.Resolve<UIMainMenuRootViewModel>();
            mainMenuViewModelContainer.Resolve<SomeMainMenuService>();

            var uiScene = Instantiate(_mainMenuUIRootPrefab);
            mainMenuContainer.Resolve<UIRootView>().AttachSceneUI(uiScene.gameObject);

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