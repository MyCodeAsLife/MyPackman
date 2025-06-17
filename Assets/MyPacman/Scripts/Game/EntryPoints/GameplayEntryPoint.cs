using R3;
using UnityEngine;

namespace MyPacman
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private DIContainer _sceneContainer;
        //private UIGameplayRootBinder _uiScene;
        //private WorldGameplayRootBinder _worldGameplayRootBinder;

        public Observable<SceneExitParams> Run(SceneEnterParams sceneEnterParams, DIContainer sceneContainer)
        {
            _sceneContainer = sceneContainer;
            GameplayEnterParams gameplayEnterParams = sceneEnterParams.As<GameplayEnterParams>();

            // Регистрация всего необходимого для данной сцены
            var gameplayRegistartions = new GameplayRegistrations(_sceneContainer, gameplayEnterParams);    // Регистрируем все сервисы необходимые для сцены
            //GameplayRegistrations.Register(_sceneContainer, gameplayEnterParams);   // Регистрируем все сервисы необходимые для сцены
            //var gameplayViewModelsContainer = new DIContainer(_sceneContainer);     // Создаем отдельный контейнер для ViewModel's
            //GameplayViewModelRegistartions.Register(gameplayViewModelsContainer);   // Регистрируем все ViewModel's необходимые для сцены

            //InitUI(gameplayViewModelsContainer);
            //InitWorld(gameplayViewModelsContainer);

            // Заглушка
            var dummy = gameObject.AddComponent<SceneEntryPoint>();
            dummy.Run(sceneEnterParams, _sceneContainer);

            // Привязка сигнала к UI сцены (на кнопку выхода в MainMenu)
            var exitParams = CreateExitParams();
            //_uiScene.Bind(_sceneContainer.Resolve<UIGameplayRootViewModel>());
            var exitToMainMenuSceneSignal = ConfigurateExitSignal(exitParams);
            return exitToMainMenuSceneSignal; // Возвращаем преобразованный сигнал
        }

        //private void CreateViewRootBinder(DIContainer gameplayViewModelsContainer)
        //{
        //    _worldGameplayRootBinder = transform.AddComponent<WorldGameplayRootBinder>();
        //    _worldGameplayRootBinder.Bind(gameplayViewModelsContainer.Resolve<WorldGameplayRootViewModel>());
        //}

        //// Можно выделить в шаблон (в MainMenuEntryPoint похожая функция)
        //private void CreateUIRootBinder()        // Создаем UIGameplayRootBinder
        //{
        //    var uiScenePrefab = Resources.Load<UIGameplayRootBinder>(GameConstants.UIGameplayFullPath);
        //    _uiScene = Instantiate(uiScenePrefab);
        //    var uiRoot = _sceneContainer.Resolve<UIRootView>();
        //    uiRoot.AttachSceneUI(_uiScene.gameObject);
        //}

        private Observable<SceneExitParams> ConfigurateExitSignal(SceneExitParams exitParams)
        {
            //var exitSceneSignalSubj = _sceneContainer.Resolve<Subject<R3.Unit>>(GameConstants.ExitSceneRequestTag);

            var exitSceneSignalSubj = new Subject<R3.Unit>();       // Заглушка
            // Преобразовываем сигнал выхода со сцены, чтобы он возвращал значение GameplayExitParams
            var exitToMainMenuSceneSignal = exitSceneSignalSubj.Select(_ => exitParams);
            return exitToMainMenuSceneSignal;
        }

        private SceneExitParams CreateExitParams()
        {
            // Создаем\конфигурируем параметры выхода с текущей сцены
            var mainMenuEnterParams = new MainMenuEnterParams("Some params");
            var exitParams = new SceneExitParams(mainMenuEnterParams);
            return exitParams;
        }

        //private void InitWorld(DIContainer viewsContainer)
        //{
        //    //CreateViewRootBinder(viewsContainer);
        //}

        //private void InitUI(DIContainer viewsContainer)
        //{
        //    //CreateUIRootBinder();

        //    //// Запрашиваем рутовую вьюмодель и пихаем ее в биндер, который создали
        //    //var uiSceneRootViewModel = viewsContainer.Resolve<UIGameplayRootViewModel>();
        //    //_uiScene.Bind(uiSceneRootViewModel);

        //    //// For test Можно открывать окошки
        //    //var uiManager = viewsContainer.Resolve<GameplayUIManager>();
        //    //uiManager.OpenScreenGameplay();
        //}
    }
}