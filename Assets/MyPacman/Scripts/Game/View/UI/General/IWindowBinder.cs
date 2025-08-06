namespace MyPacman
{
    // Для хранения в списке, так как дженерики нельзя хранить в списке
    public interface IWindowBinder
    {
        void Bind(WindowViewModel windowView);
        void Close();
    }
}