using R3;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class SceneEntryPoint : MonoBehaviour
    {
        private DIContainer _sceneContainer;

        public Observable<SceneExitParams> Run(SceneEnterParams sceneEnterParams, DIContainer sceneContainer)
        {
            _sceneContainer = sceneContainer;
            GameplayEnterParams gameplayEnterParams = sceneEnterParams.As<GameplayEnterParams>();

            var gameplayRegistartions = new GameplayRegistrations(_sceneContainer, gameplayEnterParams);    // Регистрируем все сервисы необходимые для сцены
            // Регистрация всего необходимого для данной сцены
            //GameplayRegistrations.Register(_sceneContainer, gameplayEnterParams);   // Регистрируем все сервисы необходимые для сцены
            var gameplayViewModelsContainer = new DIContainer(_sceneContainer);     // Создаем отдельный контейнер для ViewModel's
            //GameplayViewModelRegistartions.Register(gameplayViewModelsContainer);   // Регистрируем все ViewModel's необходимые для сцены

            //_sceneContainer.RegisterFactory(_ => new CharacterFactory());
            //_sceneContainer.RegisterFactory<Pacman>(c => c.Resolve<CharacterFactory>().CreatePacman(Vector3.zero));
            //_sceneContainer.RegisterFactory<PacmanView>(c => c.Resolve<CharacterFactory>().CreatePacmanTest(Vector3.zero));
            //_sceneContainer.RegisterFactory<Ghost>(c => c.Resolve<CharacterFactory>().CreateGhost(Vector3.zero));

            //_sceneContainer.RegisterInstance<ILevelConfig>(new NormalLevelConfig());

            //InitUI(gameplayViewModelsContainer);
            //InitWorld(gameplayViewModelsContainer);

            InitializeCamera();
            CreateScene();

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

        private void CreateScene()
        {
            var sceneFrame = new GameObject("Level");
            sceneFrame.AddComponent<Grid>();

            CreateWallFrame(sceneFrame.transform);
            //CreatePelletFrame(sceneFrame.transform);
            //CreateFruitFrame(sceneFrame.transform);      // Выпилить

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

        private void InitializeCamera()
        {
            const float OffsetFromScreenAspectRatio = 16f / 9f;                 // Magic

            var map = _sceneContainer.Resolve<ILevelConfig>().Map;              // Данные получать на основе GameState
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