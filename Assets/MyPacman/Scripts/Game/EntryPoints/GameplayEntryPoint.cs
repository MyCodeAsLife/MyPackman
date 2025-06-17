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

            // ����������� ����� ������������ ��� ������ �����
            var gameplayRegistartions = new GameplayRegistrations(_sceneContainer, gameplayEnterParams);    // ������������ ��� ������� ����������� ��� �����
            //GameplayRegistrations.Register(_sceneContainer, gameplayEnterParams);   // ������������ ��� ������� ����������� ��� �����
            //var gameplayViewModelsContainer = new DIContainer(_sceneContainer);     // ������� ��������� ��������� ��� ViewModel's
            //GameplayViewModelRegistartions.Register(gameplayViewModelsContainer);   // ������������ ��� ViewModel's ����������� ��� �����

            //InitUI(gameplayViewModelsContainer);
            //InitWorld(gameplayViewModelsContainer);

            // ��������
            var dummy = gameObject.AddComponent<SceneEntryPoint>();
            dummy.Run(sceneEnterParams, _sceneContainer);

            // �������� ������� � UI ����� (�� ������ ������ � MainMenu)
            var exitParams = CreateExitParams();
            //_uiScene.Bind(_sceneContainer.Resolve<UIGameplayRootViewModel>());
            var exitToMainMenuSceneSignal = ConfigurateExitSignal(exitParams);
            return exitToMainMenuSceneSignal; // ���������� ��������������� ������
        }

        //private void CreateViewRootBinder(DIContainer gameplayViewModelsContainer)
        //{
        //    _worldGameplayRootBinder = transform.AddComponent<WorldGameplayRootBinder>();
        //    _worldGameplayRootBinder.Bind(gameplayViewModelsContainer.Resolve<WorldGameplayRootViewModel>());
        //}

        //// ����� �������� � ������ (� MainMenuEntryPoint ������� �������)
        //private void CreateUIRootBinder()        // ������� UIGameplayRootBinder
        //{
        //    var uiScenePrefab = Resources.Load<UIGameplayRootBinder>(GameConstants.UIGameplayFullPath);
        //    _uiScene = Instantiate(uiScenePrefab);
        //    var uiRoot = _sceneContainer.Resolve<UIRootView>();
        //    uiRoot.AttachSceneUI(_uiScene.gameObject);
        //}

        private Observable<SceneExitParams> ConfigurateExitSignal(SceneExitParams exitParams)
        {
            //var exitSceneSignalSubj = _sceneContainer.Resolve<Subject<R3.Unit>>(GameConstants.ExitSceneRequestTag);

            var exitSceneSignalSubj = new Subject<R3.Unit>();       // ��������
            // ��������������� ������ ������ �� �����, ����� �� ��������� �������� GameplayExitParams
            var exitToMainMenuSceneSignal = exitSceneSignalSubj.Select(_ => exitParams);
            return exitToMainMenuSceneSignal;
        }

        private SceneExitParams CreateExitParams()
        {
            // �������\������������� ��������� ������ � ������� �����
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

        //    //// ����������� ������� ��������� � ������ �� � ������, ������� �������
        //    //var uiSceneRootViewModel = viewsContainer.Resolve<UIGameplayRootViewModel>();
        //    //_uiScene.Bind(uiSceneRootViewModel);

        //    //// For test ����� ��������� ������
        //    //var uiManager = viewsContainer.Resolve<GameplayUIManager>();
        //    //uiManager.OpenScreenGameplay();
        //}
    }
}