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

            // Регистрация всего необходимого для данной сцены
            var viewModelsContainer = new DIContainer(_sceneContainer);         // Создаем отдельный контейнер для ViewModel's
            new GameplayRegistrations(_sceneContainer, viewModelsContainer, gameplayEnterParams);            // Регистрируем все сервисы необходимые для сцены
            new GameplayViewModelRegistartions(viewModelsContainer);            // Регистрируем все ViewModel's необходимые для сцены

            InitCamera(gameplayEnterParams.LevelConfig.Map);
            CreateScene(gameplayEnterParams.LevelConfig);

            InitWorld(viewModelsContainer);
            InitUI(viewModelsContainer);

            // Привязка сигнала к UI сцены (на кнопку выхода в MainMenu)
            var exitParams = CreateExitParams();
            var exitToMainMenuSceneSignal = ConfigurateExitSignal(exitParams);
            return exitToMainMenuSceneSignal; // Возвращаем преобразованный сигнал
        }

        private void InitWorld(DIContainer viewsContainer)
        {
            var mapHandler = _sceneContainer.Resolve<HandlerOfPickedEntities>();
            CreateViewRootBinder(viewsContainer);
            var player = _sceneContainer.Resolve<PacmanMovementService>();
            var ghostsStateHandler = _sceneContainer.Resolve<EntitiesStateHandler>();
        }

        private void InitUI(DIContainer viewsContainer)
        {
            CreateUIRootBinder();
            // Запрашиваем\создаеам рутовую вьюмодель и пихаем ее в биндер, который создали
            _uiScene.Bind(viewsContainer.Resolve<UIGameplayRootViewModel>());

            // Вынести создание сервиса вслываюших текстовых сообщений?
            var uiManager = viewsContainer.Resolve<GameplayUIManager>();
            //var scoringService = viewsContainer.Resolve<ScoringService>();
            //_sceneContainer.RegisterInstance(new TextPopupService(uiManager, scoringService));

            // Создание UIGameplay
            uiManager.OpenUIGameplay();        // Нужен ли функционал закрытия/скрытия ui?  // Зарегестрировать в контейнер?

            // Инициализация управления (вынести отсюда)
            var inputHandler = _sceneContainer.Resolve<GameplayInputActionsHandler>();
        }

        private void CreateViewRootBinder(DIContainer gameplayViewModelsContainer)
        {
            _worldGameplayRootBinder = gameObject.AddComponent<WorldGameplayRootBinder>();
            _worldGameplayRootBinder.Bind(gameplayViewModelsContainer.Resolve<WorldGameplayRootViewModel>());
        }

        // Можно выделить в шаблон (в MainMenuEntryPoint похожая функция)
        private void CreateUIRootBinder()        // Создаем UIGameplayRootBinder
        {
            var uiScenePrefab = Resources.Load<UIGameplayRootBinder>(GameConstants.UIGameplayFullPath);
            _uiScene = Instantiate(uiScenePrefab);
            var uiRoot = _sceneContainer.Resolve<UIRootView>();
            uiRoot.AttachSceneUI(_uiScene.gameObject);
        }

        private Observable<SceneExitParams> ConfigurateExitSignal(SceneExitParams exitParams)
        {
            var exitSceneSignalSubj = _sceneContainer.Resolve<Subject<R3.Unit>>(GameConstants.SceneExitRequestTag);
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

        private void CreateScene(ILevelConfig levelConfig)
        {
            var sceneFrame = new GameObject("Level");                               // Magic
            sceneFrame.AddComponent<Grid>();

            CreateWallFrame(sceneFrame.transform);

            var levelConstructor = _sceneContainer.Resolve<LevelConstructor>();
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

        private void InitCamera(int[,] map)     // Вынуть размер карты из mapHandler?
        {
            const float OffsetFromScreenAspectRatio = 16f / 9f;                 // Magic    Получить разрешение с камеры и вставить в данную формулу

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