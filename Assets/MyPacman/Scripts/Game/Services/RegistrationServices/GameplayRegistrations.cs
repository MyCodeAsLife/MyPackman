using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class GameplayRegistrations
    {
        public GameplayRegistrations(DIContainer sceneContainer, GameplayEnterParams gameplayEnterParams)
        {
            sceneContainer.RegisterFactory(_ => new EntityBinderFactory()).AsSingle();
            sceneContainer.RegisterInstance(gameplayEnterParams.LevelConfig);

            // Перед данной регистрацией нужно чтобы состояние уровня\карты уже было создано
            var gameStateService = sceneContainer.Resolve<IGameStateService>();
            var entities = gameStateService.GameState.Map.Value.Entities;
            sceneContainer.RegisterFactory(_ => new WorldGameplayRootViewModel(entities)).AsSingle();      // Регистрацию вынести?

            sceneContainer.RegisterFactory(_ => new LevelCreator(sceneContainer, gameplayEnterParams.LevelConfig)).AsSingle();

            sceneContainer.RegisterFactory(_ => new MapHandlerService(
                    gameStateService.GameState,
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
        }
    }
}
