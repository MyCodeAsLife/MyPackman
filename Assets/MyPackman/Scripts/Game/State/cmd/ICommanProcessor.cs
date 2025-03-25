namespace Game.State.cmd
{
    public interface ICommanProcessor
    {
        public void RegisterHandler<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand;
        public bool Procces<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
