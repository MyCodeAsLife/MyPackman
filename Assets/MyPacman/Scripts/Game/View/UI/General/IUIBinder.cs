namespace MyPacman
{
    // Для хранения в списке, так как дженерики нельзя хранить в списке
    public interface IUIBinder
    {
        void Bind(UIViewModel windowView);
        void Close();
    }
}