using R3;
using UnityEngine;

namespace MyPacman
{
    public class Ghost : Edible, IMovable
    {
        public Ghost(GhostData data) : base(data)
        {
            IsMoving = new ReactiveProperty<bool>(data.IsMoving);
            IsMoving.Subscribe(isMoving => data.IsMoving = isMoving);
            Direction = new ReactiveProperty<Vector2>(data.Direction);
            Direction.Subscribe(direction => data.Direction = new Vector2Int((int)direction.x, (int)direction.y));
        }

        public ReactiveProperty<bool> IsMoving { get; private set; }

        public ReactiveProperty<Vector2> Direction { get; private set; }
    }
}
