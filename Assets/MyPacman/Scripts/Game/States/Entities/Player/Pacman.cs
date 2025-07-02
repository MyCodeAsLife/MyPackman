using R3;
using System;
using UnityEngine;

namespace MyPacman
{
    public class Pacman : Entity
    {
        //public ReactiveProperty<float> StartPositionX;
        //public ReactiveProperty<float> StartPositionY;
        public ReactiveProperty<Vector2> Direction;     // Добавить промежуточный класс для персонажей и вынести туда

        private float _startPositionX;
        private float _startPositionY;

        public Func<Vector2> GetCurrentPosition;        // Вынести в Entity?

        public Pacman(PacmanData pacmanData) : base(pacmanData)
        {
            _startPositionX = pacmanData.StartPositionX;
            //StartPositionX.Subscribe(newPosition => pacmanData.StartPositionX = newPosition);

            _startPositionY = pacmanData.StartPositionY;
            //StartPositionY.Subscribe(newPosition => pacmanData.PositionY = newPosition);

            Direction = new ReactiveProperty<Vector2>(pacmanData.Direction);
            Direction.Subscribe(direction => pacmanData.Direction = new Vector2Int((int)direction.x, (int)direction.y));
        }

        public Vector2 StartPosition
        {
            get
            {
                return new Vector2(_startPositionX, _startPositionY);
            }
        }

        public void PassPositionRequestFunction(Func<Vector2> getCurrentPosition)
        {
            GetCurrentPosition = getCurrentPosition;
        }
    }
}
