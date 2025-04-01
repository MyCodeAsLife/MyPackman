using DI;
using Game.Common;
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
            InitWorld(gameplayViewModelsContainer);
            InitUI(gameplayViewModelsContainer);

            //for tests
            Debug.Log($"Gameplay entry point: Run gameplay scene. " +
                      $"Scene name: {sceneEnterParams.SceneName}. " +
                      $"Map ID: {sceneEnterParams.MapId}");

            // Создаем объект для возвращаемых параметров и заполняем его
            var mainMenuEnterParams = new MainMenuEnterParams("Result");
            // Заворачиваем созданный выше объект в новый объект
            var exitParams = new GameplayExitParams(mainMenuEnterParams);
            var exitSceneRequest = gameplayContainer.Resolve<Subject<Unit>>(Constants.EXIT_SCENE_REQUEST_TAG);
            // Заворачиваем наш объект в объект сигнала с которого можно только считывать
            var exitToMainMenuSceneSignal = exitSceneRequest.Select(_ => exitParams);
            // И возвращаем его
            return exitToMainMenuSceneSignal;
        }

        private void InitWorld(DIContainer viewsContainer)
        {
            _worldRootBinder.Bind(viewsContainer.Resolve<WorldGameplayRootViewModel>());
        }

        private void InitUI(DIContainer viewsContainer)
        {
            // Создаем UI сцены из префаба и прикрепляем его к корневому UIRoot
            var uiRoot = viewsContainer.Resolve<UIRootView>();
            var uiSceneRootBinder = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiSceneRootBinder.gameObject);

            // Запрашиваем рутовую вьюмодкль и пихаем ее в биндер, который создали
            var uiSceneRootViewModel = viewsContainer.Resolve<UIGameplayRootViewModel>();
            uiSceneRootBinder.Bind(uiSceneRootViewModel);

            // For tests
            var uiManager = viewsContainer.Resolve<GameplayUIManager>();
            uiManager.OpenScreenGameplay();
        }
    }
}
