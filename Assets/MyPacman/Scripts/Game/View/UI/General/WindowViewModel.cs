using R3;

namespace MyPacman
{
    public abstract class WindowViewModel : UIViewModel
    {
        public Observable<WindowViewModel> CloseRequested => _closeRequested;

        private readonly Subject<WindowViewModel> _closeRequested = new();

        public void RequestClose()  // Запрос на закрытие окна, будет вызыватся из монобеха
        {
            _closeRequested.OnNext(this);
        }
    }
}