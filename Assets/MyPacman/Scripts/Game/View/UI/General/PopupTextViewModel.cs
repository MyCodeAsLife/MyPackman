using System;

namespace MyPacman
{
    public abstract class PopupTextViewModel : IDisposable
    {
        public abstract string Id { get; }  // Имя для поиска\выбора префаба

        public virtual void Dispose() { }
    }
}
