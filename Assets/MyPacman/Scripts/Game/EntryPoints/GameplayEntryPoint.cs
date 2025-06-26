using R3;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private DIContainer _sceneContainer;
        //private UIGameplayRootBinder _uiScene;
        private WorldGameplayRootBinder _worldGameplayRootBinder;

        public Observable<SceneExitParams> Run(SceneEnterParams sceneEnterParams, DIContainer sceneContainer)
        {
            _sceneContainer = sceneContainer;
            GameplayEnterParams gameplayEnterParams = sceneEnterParams.As<GameplayEnterParams>();

            // ����������� ����� ������������ ��� ������ �����
            var gameplayRegistartions = new GameplayRegistrations(_sceneContainer, gameplayEnterParams);    // ������������ ��� ������� ����������� ��� �����
            //GameplayRegistrations.Register(_sceneContainer, gameplayEnterParams);     // ������������ ��� ������� ����������� ��� �����
            var gameplayViewModelsContainer = new DIContainer(_sceneContainer);         // ������� ��������� ��������� ��� ViewModel's
            //GameplayViewModelRegistartions.Register(gameplayViewModelsContainer);     // ������������ ��� ViewModel's ����������� ��� �����

            // Old
            InitCamera();
            CreateScene();

            // New
            InitUI(gameplayViewModelsContainer);
            InitWorld(gameplayViewModelsContainer);

            // �������� ������� � UI ����� (�� ������ ������ � MainMenu)
            var exitParams = CreateExitParams();
            //_uiScene.Bind(_sceneContainer.Resolve<UIGameplayRootViewModel>());
            var exitToMainMenuSceneSignal = ConfigurateExitSignal(exitParams);
            return exitToMainMenuSceneSignal; // ���������� ��������������� ������
        }

        private void CreateViewRootBinder(DIContainer gameplayViewModelsContainer)
        {
            _worldGameplayRootBinder = gameObject.AddComponent<WorldGameplayRootBinder>();
            _worldGameplayRootBinder.Bind(gameplayViewModelsContainer.Resolve<WorldGameplayRootViewModel>());
        }

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

        private void InitWorld(DIContainer viewsContainer)
        {
            //CreateViewRootBinder(viewsContainer);
        }

        private void InitUI(DIContainer viewsContainer)
        {
            //CreateUIRootBinder();

            //// ����������� ������� ��������� � ������ �� � ������, ������� �������
            //var uiSceneRootViewModel = viewsContainer.Resolve<UIGameplayRootViewModel>();
            //_uiScene.Bind(uiSceneRootViewModel);

            //// For test ����� ��������� ������
            //var uiManager = viewsContainer.Resolve<GameplayUIManager>();
            //uiManager.OpenScreenGameplay();
        }

        private void CreateScene()
        {
            var sceneFrame = new GameObject("Level");
            sceneFrame.AddComponent<Grid>();

            CreateWallFrame(sceneFrame.transform);
            CreatePelletFrame(sceneFrame.transform);
            CreateNodeFrame(sceneFrame.transform);      // ��������

            var levelConstarctor = new LevelConstructor(_sceneContainer);
            levelConstarctor.ConstructLevel();
        }

        private void CreateWallFrame(Transform parent)
        {
            var walls = new GameObject(GameConstants.Obstacle);
            walls.transform.parent = parent;
            walls.layer = LayerMask.NameToLayer(GameConstants.Obstacle);
            var obstacleTilemap = walls.AddComponent<Tilemap>();
            _sceneContainer.RegisterInstance(GameConstants.Obstacle, obstacleTilemap);
            walls.AddComponent<TilemapRenderer>();
            var wallsCollider = walls.AddComponent<TilemapCollider2D>();
        }

        private void CreatePelletFrame(Transform parent)
        {
            var pellets = new GameObject(GameConstants.Pellet);
            pellets.transform.parent = parent;
            pellets.layer = LayerMask.NameToLayer(GameConstants.Pellet);
            var pelletsTilemap = pellets.AddComponent<Tilemap>();
            _sceneContainer.RegisterInstance(GameConstants.Pellet, pelletsTilemap);
            var pelletsCollider = pellets.AddComponent<TilemapCollider2D>();
            pelletsCollider.excludeLayers = GameConstants.LayerMaskEverything;
            pellets.AddComponent<TilemapRenderer>();
        }

        // ��������
        private void CreateNodeFrame(Transform parent)
        {
            var nodes = new GameObject(GameConstants.Node);
            nodes.transform.parent = parent;
            nodes.layer = LayerMask.NameToLayer(GameConstants.Node);
            var nodesTilemap = nodes.AddComponent<Tilemap>();
            nodes.AddComponent<TilemapCollider2D>();
            _sceneContainer.RegisterInstance(GameConstants.Node, nodesTilemap);
        }

        private void InitCamera()
        {
            const float OffsetFromScreenAspectRatio = 16f / 9f;                 // Magic

            var map = _sceneContainer.Resolve<ILevelConfig>().Map;              // ������ �������� �� ������ GameState
            float y = map.GetLength(0) * GameConstants.GridCellSize * GameConstants.Half;
            float x = map.GetLength(1) * GameConstants.GridCellSize * GameConstants.Half;

            Camera.main.transform.position
                = new Vector3(x, -y + GameConstants.GameplayInformationalPamelHeight, Camera.main.transform.position.z);
            float size = y;

            if (x > y)
                size = x / OffsetFromScreenAspectRatio - GameConstants.GameplayInformationalPamelHeight;

            Camera.main.orthographicSize
                = size + (GameConstants.GameplayInformationalPamelHeight * OffsetFromScreenAspectRatio);
        }
    }
}