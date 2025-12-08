using R3;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class GameplayRegistrations
    {
        public GameplayRegistrations(
            DIContainer sceneContainer,
            DIContainer viewModelsContainer,
            GameplayEnterParams gameplayEnterParams)
        {
            sceneContainer.RegisterInstance(gameplayEnterParams.LevelConfig);

            // Перед данной регистрацией нужно чтобы состояние уровня\карты уже было создано
            var gameStateService = sceneContainer.Resolve<IGameStateService>();
            var entities = gameStateService.GameState.Map.Value.Entities;

            // Регистрируем сигнал запрашивающий выход в MainMenu
            sceneContainer.RegisterInstance(GameConstants.SceneExitRequestTag, new Subject<Unit>());
            // Регистрация сигнала окончания анимации смерти пакмана
            //sceneContainer.RegisterInstance(GameConstants.RequestTagWhenDeathAnimationEnds, new Subject<Unit>());

            sceneContainer.RegisterFactory(_ => new PlayerInputActions()).AsSingle();
            sceneContainer.RegisterFactory(_ => new WorldGameplayRootViewModel(entities)).AsSingle();

            sceneContainer.RegisterFactory(_ => new LevelConstructor(
                sceneContainer,
                gameplayEnterParams.LevelConfig
                )).AsSingle();

            sceneContainer.RegisterFactory(_ => new PickableEntityHandler(
                    gameStateService.GameState,
                    sceneContainer.Resolve<ILevelConfig>(),
                    sceneContainer.Resolve<Tilemap>(GameConstants.Obstacle),
                    sceneContainer.Resolve<PacmanMovementService>(),
                    sceneContainer.Resolve<TimeService>()
                )).AsSingle();

            sceneContainer.RegisterFactory(_ => new PacmanMovementService(
                    sceneContainer.Resolve<Entity>(EntityType.Pacman.ToString()) as Pacman,
                    sceneContainer.Resolve<PlayerInputActions>(),
                    sceneContainer.Resolve<IGameStateService>(),
                    sceneContainer.Resolve<ILevelConfig>(),
                    sceneContainer.Resolve<TimeService>()
                )).AsSingle();

            // Перенести регистрацию во вьюмодели
            sceneContainer.RegisterFactory<IUIGameplayViewModel>(_ => new UIGameplayViewModel(
                    gameStateService.GameState
                )).AsSingle();

            sceneContainer.RegisterFactory(_ => new EntitiesStateHandler(
                    entities,
                    sceneContainer.Resolve<Entity>(EntityType.Pacman.ToString()) as Pacman,
                    gameStateService.GameState.LifePoints,
                    gameStateService.GameState.Map.CurrentValue.GetSpawnPosition,
                    sceneContainer.Resolve<TimeService>(),
                    sceneContainer.Resolve<PickableEntityHandler>(),
                    sceneContainer.Resolve<ILevelConfig>(),
                    gameStateService.GameState.LevelTimeHasPassed,
                    gameStateService.GameState.Map.Value.TimeUntilEndOfGlobalBehaviorMode,
                    gameStateService.GameState.Map.Value.GlobalStateOfBehavior
                )).AsSingle();

            sceneContainer.RegisterFactory(_ => new ScoringService(
                    gameStateService.GameState,
                    sceneContainer.Resolve<PickableEntityHandler>(),
                    sceneContainer.Resolve<EntitiesStateHandler>(),
                    sceneContainer.Resolve<IUIGameplayViewModel>()
                )).AsSingle();

            sceneContainer.RegisterFactory(_ => new GameplayInputActionsHandler(
                    viewModelsContainer.Resolve<GameplayUIManager>(),
                    sceneContainer.Resolve<PlayerInputActions>(),
                    sceneContainer.Resolve<PacmanMovementService>(),
                    sceneContainer.Resolve<TextPopupService>(),
                    sceneContainer.Resolve<TimeService>(),
                    gameStateService.GameState.Map.CurrentValue.FruitSpawnPos
                )).AsSingle();

            sceneContainer.RegisterFactory(_ => new TextPopupService(
                viewModelsContainer.Resolve<GameplayUIManager>(),
                sceneContainer.Resolve<ScoringService>()
                )).AsSingle();
        }
    }
}