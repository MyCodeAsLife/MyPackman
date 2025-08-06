namespace MyPacman
{
    // Для хранения в списке, так как дженерики нельзя хранить в списке
    public interface IPopupTextBinder
    {
        void Bind(PopupTextViewModel popupTextView);
        void Close();
    }
}
