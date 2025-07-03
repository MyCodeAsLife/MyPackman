namespace MyPacman
{
    public class GameplayRegistrations
    {
        public GameplayRegistrations(DIContainer sceneContainer, GameplayEnterParams gameplayEnterParams)
        {
            sceneContainer.RegisterFactory(_ => new EntityBinderFactory()).AsSingle();
            sceneContainer.RegisterInstance(gameplayEnterParams.LevelConfig);
            //sceneContainer.RegisterInstance<ILevelConfig>(new NormalLevelConfig());

            // Перед данной регистрацией нужно чтобы состояние уровня\карты уже было создано
            var gameStateService = sceneContainer.Resolve<IGameStateService>();
            var entities = gameStateService.GameState.Map.Value.Entities;
            sceneContainer.RegisterFactory(_ => new WorldGameplayRootViewModel(entities)).AsSingle();      // Регистрацию вынести?
        }
    }
}
