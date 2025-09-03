using R3;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private DIContainer _sceneContainer;
        private UIGameplayRootBinder _uiScene;
        private WorldGameplayRootBinder _worldGameplayRootBinder;

        public Observable<SceneExitParams> Run(SceneEnterParams sceneEnterParams, DIContainer sceneContainer)
        {
            _sceneContainer = sceneContainer;
            GameplayEnterParams gameplayEnterParams = sceneEnterParams.As<GameplayEnterParams>();

            // ����������� ����� ������������ ��� ������ �����
            var viewModelsContainer = new DIContainer(_sceneContainer);         // ������� ��������� ��������� ��� ViewModel's
            new GameplayRegistrations(_sceneContainer, viewModelsContainer, gameplayEnterParams);            // ������������ ��� ������� ����������� ��� �����
            new GameplayViewModelRegistartions(viewModelsContainer);            // ������������ ��� ViewModel's ����������� ��� �����

            InitCamera(gameplayEnterParams.LevelConfig.Map);
            CreateScene(gameplayEnterParams.LevelConfig);

            InitWorld(viewModelsContainer);
            InitUI(viewModelsContainer);

            // �������� ������� � UI ����� (�� ������ ������ � MainMenu)
            var exitParams = CreateExitParams();
            var exitToMainMenuSceneSignal = ConfigurateExitSignal(exitParams);
            return exitToMainMenuSceneSignal; // ���������� ��������������� ������
        }

        private void InitWorld(DIContainer viewsContainer)
        {
            var mapHandler = _sceneContainer.Resolve<MapHandlerService>();
            CreateViewRootBinder(viewsContainer);
            var player = _sceneContainer.Resolve<PlayerMovementService>();
            //var scoringService = _sceneContainer.Resolve<ScoringService>();           // �����        
            var ghostsStateHandler = _sceneContainer.Resolve<GhostsStateHandler>();
        }

        private void InitUI(DIContainer viewsContainer)
        {
            CreateUIRootBinder();
            // �����������\�������� ������� ��������� � ������ �� � ������, ������� �������
            _uiScene.Bind(viewsContainer.Resolve<UIGameplayRootViewModel>());

            // ������� �������� ������� ���������� ��������� ���������?
            var uiManager = viewsContainer.Resolve<GameplayUIManager>();
            var scoringService = viewsContainer.Resolve<ScoringService>();
            _sceneContainer.RegisterInstance(new TextPopupService(uiManager, scoringService));
            // �������� UIGameplay
            uiManager.OpenUIGameplay();        // ����� �� ���������� ��������/������� ui?  // ���������������� � ���������?

            // ������������� ���������� (������� ������)
            var inputHandler = _sceneContainer.Resolve<GameplayInputActionsHandler>();
        }

        private void CreateViewRootBinder(DIContainer gameplayViewModelsContainer)
        {
            _worldGameplayRootBinder = gameObject.AddComponent<WorldGameplayRootBinder>();
            _worldGameplayRootBinder.Bind(gameplayViewModelsContainer.Resolve<WorldGameplayRootViewModel>());
        }

        // ����� �������� � ������ (� MainMenuEntryPoint ������� �������)
        private void CreateUIRootBinder()        // ������� UIGameplayRootBinder
        {
            var uiScenePrefab = Resources.Load<UIGameplayRootBinder>(GameConstants.UIGameplayFullPath);
            _uiScene = Instantiate(uiScenePrefab);
            var uiRoot = _sceneContainer.Resolve<UIRootView>();
            uiRoot.AttachSceneUI(_uiScene.gameObject);
        }

        private Observable<SceneExitParams> ConfigurateExitSignal(SceneExitParams exitParams)
        {
            var exitSceneSignalSubj = _sceneContainer.Resolve<Subject<R3.Unit>>(GameConstants.ExitSceneRequestTag);
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

        private void CreateScene(ILevelConfig levelConfig)
        {
            var sceneFrame = new GameObject("Level");                               // Magic
            sceneFrame.AddComponent<Grid>();

            CreateWallFrame(sceneFrame.transform);

            var levelCreator = _sceneContainer.Resolve<LevelConstructor>();
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

        private void InitCamera(int[,] map)     // ������ ������ ����� �� mapHandler?
        {
            const float OffsetFromScreenAspectRatio = 16f / 9f;                 // Magic    �������� ���������� � ������ � �������� � ������ �������

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