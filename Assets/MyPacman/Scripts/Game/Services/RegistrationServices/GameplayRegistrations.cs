using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class GameplayRegistrations
    {
        public GameplayRegistrations(DIContainer sceneContainer, GameplayEnterParams gameplayEnterParams)
        {
            sceneContainer.RegisterInstance(gameplayEnterParams.LevelConfig);

            // Перед данной регистрацией нужно чтобы состояние уровня\карты уже было создано
            var gameStateService = sceneContainer.Resolve<IGameStateService>();
            var entities = gameStateService.GameState.Map.Value.Entities;

            sceneContainer.RegisterFactory(_ => new WorldGameplayRootViewModel(entities)).AsSingle();      // Регистрацию вынести?

            sceneContainer.RegisterFactory(_ => new LevelCreator(sceneContainer, gameplayEnterParams.LevelConfig)).AsSingle();

            sceneContainer.RegisterFactory(_ => new MapHandlerService(
                    gameStateService.GameState,
                    sceneContainer.Resolve<ILevelConfig>(),
                    sceneContainer.Resolve<Tilemap>(GameConstants.Obstacle),
                    sceneContainer.Resolve<PlayerMovemenService>()
                )).AsSingle();

            sceneContainer.RegisterFactory(_ => new PlayerMovemenService(
                    sceneContainer.Resolve<Entity>(EntityType.Pacman.ToString()) as Pacman,
                    sceneContainer.Resolve<IGameStateService>(),
                    sceneContainer.Resolve<ILevelConfig>(),
                    sceneContainer.Resolve<TimeService>()
                )).AsSingle();

            sceneContainer.RegisterFactory(_ => new ScoringService(
                    gameStateService.GameState,
                    sceneContainer.Resolve<MapHandlerService>()
                )).AsSingle();

            sceneContainer.RegisterFactory(_ => new GhostsStateHandler(
                    entities,
                    sceneContainer.Resolve<Entity>(EntityType.Pacman.ToString()) as Pacman,
                    gameStateService.GameState.LifePoints,
                    gameStateService.GameState.Map.CurrentValue.PacmanSpawnPos,
                    sceneContainer.Resolve<TimeService>(),
                    sceneContainer.Resolve<MapHandlerService>(),
                    sceneContainer.Resolve<ILevelConfig>(),
                    sceneContainer.Resolve<IGameStateService>().GameState.Map.Value.InkySpawnPos   // Временное решение?
                )).AsSingle();
        }
    }
}
