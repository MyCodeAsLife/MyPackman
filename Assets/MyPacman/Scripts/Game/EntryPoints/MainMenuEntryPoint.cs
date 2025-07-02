using R3;
using UnityEngine;

namespace MyPacman
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        //private UIMainMenuRootBinder _uiScene;
        private DIContainer _sceneContainer;

        public Observable<MainMenuExitParams> Run(MainMenuEnterParams mainMenuEnterParams, DIContainer sceneContainer)
        {
            _sceneContainer = sceneContainer;

            // ����������� ����� ������������ ��� ������ �����
            //MainMenuRegistrations.Register(_sceneContainer, mainMenuEnterParams);   // ������������ ��� ������� ����������� ��� ����� (�������)
            var mainMenuViewModelContainer = new DIContainer(_sceneContainer);      // ������� ��������� ��������� ��� ViewModel's
            //MainMenuViewModelRegistartions.Register(mainMenuViewModelContainer);    // ������������ ��� ViewModel's ����������� ��� �����

            //// For test
            //_sceneContainer.Resolve<SomeMainMenuService>();
            //mainMenuViewModelContainer.Resolve<UIMainMenuRootViewModel>();

            //CreateUISceneBinder();

            //// ��������
            //var dummy = gameObject.AddComponent<SceneEntryPoint>();
            //dummy.Run(_sceneContainer);

            //Debug.Log($"Run MainMenu scene. Result: {mainMenuEnterParams?.EnterParams}");           //++++++++++++++++++++++

            var exitParams = CreateExitParams();
            var exitSceneSignalSubj = CreateExitSignal();
            var exitToGameplaySceneSignal = ConfigurateExitSignal(exitSceneSignalSubj, exitParams);
            return exitToGameplaySceneSignal; // ���������� ��������������� ������
        }

        private MainMenuExitParams CreateExitParams()   // �������\������������� ��������� ������ � ������� �����
        {
            // �������� ������ ������ ��� ��������\��������
            string saveFileName = "large.save";
            ILevelConfig levelConfig = new NormalLevelConfig();
            var gameplayEnterParams = new GameplayEnterParams(saveFileName, levelConfig);
            var exitParams = new MainMenuExitParams(gameplayEnterParams);
            return exitParams;
        }

        private Subject<Unit> CreateExitSignal()
        {
            // �������� ������� � �������� ��� � UI ����� (�� ������ ������ � MainMenu)
            var exitSceneSignalSubj = new Subject<Unit>();
            //_uiScene.Bind(exitSceneSignalSubj);
            return exitSceneSignalSubj;
        }

        private Observable<MainMenuExitParams> ConfigurateExitSignal(Subject<Unit> exitSceneSignalSubj,
                                                                     MainMenuExitParams exitParams)
        {
            // ��������������� ������ ������ �� �����, ����� �� ��������� �������� GameplayExitParams
            var exitToMainMenuSceneSignal = exitSceneSignalSubj.Select(_ => exitParams);
            return exitToMainMenuSceneSignal;
        }

        // ����� �������� � ������ (� GameplayEntryPoint ������� �������)
        //private void CreateUISceneBinder()        // ������� UIGameplayRootBinder
        //{
        //    var uiScenePrefab = Resources.Load<UIMainMenuRootBinder>(GameConstants.UIMainMenuFullPath);
        //    _uiScene = Instantiate(uiScenePrefab);
        //    var uiRoot = _sceneContainer.Resolve<UIRootView>();
        //    uiRoot.AttachSceneUI(_uiScene.gameObject);
        //}
    }
}