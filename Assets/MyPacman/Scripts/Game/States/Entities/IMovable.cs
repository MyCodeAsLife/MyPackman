using R3;
using UnityEngine;

namespace MyPacman
{
    public interface IMovable
    {
        public ReactiveProperty<Vector2> Direction { get; }
        public ReactiveProperty<bool> IsMoving { get; }
    }
}
