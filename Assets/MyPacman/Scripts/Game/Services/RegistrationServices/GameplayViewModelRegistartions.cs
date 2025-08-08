namespace MyPacman
{
    public class GameplayViewModelRegistartions
    {
        public GameplayViewModelRegistartions(DIContainer viewModelsContainer)
        {
            viewModelsContainer.RegisterFactory(_ => new GameplayUIManager(viewModelsContainer)).AsSingle();
        }
    }
}