using R3;
using System;
using UnityEngine;

namespace MyPacman
{
    public class Pacman : Entity, IMovable
    {
        public Func<Vector2> GetCurrentPosition;

        public Pacman(PacmanData pacmanData) : base(pacmanData)
        {
            IsMoving = new ReactiveProperty<bool>(pacmanData.IsMoving);
            IsMoving.Subscribe(isMoving => pacmanData.IsMoving = isMoving);
            Direction = new ReactiveProperty<Vector2>(pacmanData.Direction);
            Direction.Subscribe(direction => pacmanData.Direction = new Vector2Int((int)direction.x, (int)direction.y));
        }

        public ReactiveProperty<bool> IsMoving { get; private set; }

        public ReactiveProperty<Vector2> Direction { get; private set; }

        public void PassPositionRequestFunction(Func<Vector2> getCurrentPosition)
        {
            GetCurrentPosition = getCurrentPosition;
        }
    }
}
