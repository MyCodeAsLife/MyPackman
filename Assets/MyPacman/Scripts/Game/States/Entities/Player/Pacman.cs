using R3;
using System;
using UnityEngine;

namespace MyPacman
{
    public class Pacman : Entity
    {
        //public ReactiveProperty<float> PositionX;
        //public ReactiveProperty<float> PositionY;
        // new
        public ReactiveProperty<Vector2> Direction;     // Добавить промежуточный класс для персонажей и вынести туда

        public Func<Vector2> GetCurrentPosition;        // Вынести в Entity?

        public Pacman(PacmanData pacmanData) : base(pacmanData)
        {
            //PositionX = new ReactiveProperty<float>(pacmanData.PositionX);
            //PositionX.Subscribe(newPosition => pacmanData.PositionX = newPosition);

            //PositionY = new ReactiveProperty<float>(pacmanData.PositionY);
            //PositionY.Subscribe(newPosition => pacmanData.PositionY = newPosition);

            Direction = new ReactiveProperty<Vector2>(pacmanData.Direction);
            Direction.Subscribe(direction => pacmanData.Direction = new Vector2Int((int)direction.x, (int)direction.y));
        }

        public void PassPositionRequestFunction(Func<Vector2> getCurrentPosition)
        {
            GetCurrentPosition = getCurrentPosition;
        }
    }
}
