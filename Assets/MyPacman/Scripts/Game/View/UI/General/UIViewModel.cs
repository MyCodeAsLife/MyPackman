using System;

namespace MyPacman
{
    public abstract class UIViewModel : IDisposable
    {
        public abstract string Id { get; }  // Имя для поиска\выбора префаба
        public virtual void Dispose() { }
    }
}
