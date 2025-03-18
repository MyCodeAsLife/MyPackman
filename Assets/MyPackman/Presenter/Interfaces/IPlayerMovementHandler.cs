namespace Assets.MyPackman.Presenter
{
    public interface IPlayerMovementHandler
    {
        public void Tick();
        public void SetMovementDirection(int direction);
        public void RemoveMovementDirection(int direction);
    }
}
