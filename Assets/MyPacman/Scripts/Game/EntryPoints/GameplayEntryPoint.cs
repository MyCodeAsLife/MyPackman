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

            // Регистрация всего необходимого для данной сцены
            var gameplayRegistartions = new GameplayRegistrations(_sceneContainer, gameplayEnterParams);    // Регистрируем все сервисы необходимые для сцены
            //GameplayRegistrations.Register(_sceneContainer, gameplayEnterParams);     // Регистрируем все сервисы необходимые для сцены
            var gameplayViewModelsContainer = new DIContainer(_sceneContainer);         // Создаем отдельный контейнер для ViewModel's
            //GameplayViewModelRegistartions.Register(gameplayViewModelsContainer);     // Регистрируем все ViewModel's необходимые для сцены

            InitCamera(gameplayEnterParams.LevelConfig.Map);
            CreateScene(gameplayEnterParams.LevelConfig);

            // New
            InitUI(gameplayViewModelsContainer);
            InitWorld(gameplayViewModelsContainer);

            // Привязка сигнала к UI сцены (на кнопку выхода в MainMenu)
            var exitParams = CreateExitParams();
            //_uiScene.Bind(_sceneContainer.Resolve<UIGameplayRootViewModel>());
            var exitToMainMenuSceneSignal = ConfigurateExitSignal(exitParams);
            return exitToMainMenuSceneSignal; // Возвращаем преобразованный сигнал
        }

        private void CreateViewRootBinder(DIContainer gameplayViewModelsContainer)
        {
            _worldGameplayRootBinder = gameObject.AddComponent<WorldGameplayRootBinder>();
            _worldGameplayRootBinder.Bind(gameplayViewModelsContainer.Resolve<WorldGameplayRootViewModel>());
        }

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

        private void InitWorld(DIContainer viewsContainer)
        {
            //// Перед данной регистрацией нужно чтобы состояние уровня\карты уже было создано
            //var gameStateService = _sceneContainer.Resolve<IGameStateService>();
            //var entities = gameStateService.GameState.Map.Value.Entities;
            //_sceneContainer.RegisterFactory(_ => new WorldGameplayRootViewModel(entities)).AsSingle();      // Регистрацию вынести?

            //_worldGameplayRootBinder = gameObject.AddComponent<WorldGameplayRootBinder>();
            //_worldGameplayRootBinder.Bind(viewsContainer.Resolve<WorldGameplayRootViewModel>());

            CreateViewRootBinder(viewsContainer);
            var playerControl = new PlayerMovemenService();

            // Это все впихнуть в вызов playerControl.Run
            var pacman = _sceneContainer.Resolve<Entity>(EntityType.Pacman.ToString());
            var gameStateService = _sceneContainer.Resolve<IGameStateService>();
            var levelConfig = _sceneContainer.Resolve<ILevelConfig>();
            var mapHandler = _sceneContainer.Resolve<MapHandlerService>();
            var timeService = _sceneContainer.Resolve<TimeService>();

            playerControl.Run(pacman as Pacman, gameStateService, levelConfig, mapHandler, timeService);
        }

        private void InitUI(DIContainer viewsContainer)
        {
            //CreateUIRootBinder();

            //// Запрашиваем рутовую вьюмодель и пихаем ее в биндер, который создали
            //var uiSceneRootViewModel = viewsContainer.Resolve<UIGameplayRootViewModel>();
            //_uiScene.Bind(uiSceneRootViewModel);

            //// For test Можно открывать окошки
            //var uiManager = viewsContainer.Resolve<GameplayUIManager>();
            //uiManager.OpenScreenGameplay();
        }

        private void CreateScene(ILevelConfig levelConfig)
        {
            var sceneFrame = new GameObject("Level");
            sceneFrame.AddComponent<Grid>();

            CreateWallFrame(sceneFrame.transform);
            _sceneContainer.RegisterInstance(new LevelCreator(_sceneContainer, levelConfig));

            //CreatePelletFrame(sceneFrame.transform);
            //CreateFruitFrame(sceneFrame.transform);      // Выпилить

            //var levelConstarctor = new LevelConstructor(_sceneContainer);
            //levelConstarctor.ConstructLevel();
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

        //private void CreatePelletFrame(Transform parent)
        //{
        //    var pellets = new GameObject(GameConstants.Pellet);
        //    pellets.transform.parent = parent;
        //    pellets.layer = LayerMask.NameToLayer(GameConstants.Pellet);
        //    var pelletsTilemap = pellets.AddComponent<Tilemap>();
        //    _sceneContainer.RegisterInstance(GameConstants.Pellet, pelletsTilemap);
        //    var pelletsCollider = pellets.AddComponent<TilemapCollider2D>();
        //    pelletsCollider.excludeLayers = GameConstants.LayerMaskEverything;
        //    pellets.AddComponent<TilemapRenderer>();
        //}

        //// Выпилить
        //private void CreateFruitFrame(Transform parent)
        //{
        //    var Fruits = new GameObject(GameConstants.Fruit);
        //    Fruits.transform.parent = parent;
        //    Fruits.layer = LayerMask.NameToLayer(GameConstants.Fruit);
        //    var FruitsTilemap = Fruits.AddComponent<Tilemap>();
        //    Fruits.AddComponent<TilemapCollider2D>();
        //    _sceneContainer.RegisterInstance(GameConstants.Fruit, FruitsTilemap);
        //}

        private void InitCamera(int[,] map)
        {
            const float OffsetFromScreenAspectRatio = 16f / 9f;                 // Magic

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