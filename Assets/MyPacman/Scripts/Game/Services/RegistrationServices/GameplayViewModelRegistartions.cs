namespace MyPacman
{
    public class GameplayViewModelRegistartions
    {
        public GameplayViewModelRegistartions(DIContainer viewModelsContainer)
        {
            viewModelsContainer.RegisterFactory(_ => new GameplayUIManager(viewModelsContainer)).AsSingle();
            viewModelsContainer.RegisterFactory(_ => new UIGameplayRootViewModel(
                viewModelsContainer.Resolve<TimeService>()
                )).AsSingle();
        }
    }
}