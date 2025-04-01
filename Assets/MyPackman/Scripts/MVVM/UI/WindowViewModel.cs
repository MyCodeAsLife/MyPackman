using R3;
using System;

namespace MVVM.UI
{
    public abstract class WindowViewModel : IDisposable
    {
        public Observable<WindowViewModel> CloseRequested => _closeRequested;
        // abstract в обозначении поля ОБЯЗЫВАЕТ наследников перезаписывать данное поле
        public abstract string Id { get; }      // Тут будут пути к префабам

        private readonly Subject<WindowViewModel> _closeRequested = new();

        public void RequestClose()      // Запрос на закрытие окна, менеджеру окон
        {
            _closeRequested.OnNext(this);
        }

        public void Dispose() { }
    }
}