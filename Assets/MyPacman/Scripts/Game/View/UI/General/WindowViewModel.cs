using R3;
using System;

namespace MyPacman
{
    public abstract class WindowViewModel : IDisposable
    {
        public Observable<WindowViewModel> CloseRequested => _closeRequested;
        public abstract string Id { get; }  // Имя для поиска\выбора префаба

        private readonly Subject<WindowViewModel> _closeRequested = new();

        public void RequestClose()  // Запрос на закрытие окна, будет вызыватся из монобеха
        {
            _closeRequested.OnNext(this);
        }

        public virtual void Dispose() { }
    }
}