using R3;
using UnityEngine;

namespace MyPacman
{
    public class PacmanEntity : Entity
    {
        public PacmanEntity(PacmanEntityData pacmanData) : base(pacmanData)
        {
            //PositionX = new ReactiveProperty<float>(pacmanData.PositionX);
            //PositionX.Subscribe(newPosition => pacmanData.PositionX = newPosition);

            //PositionY = new ReactiveProperty<float>(pacmanData.PositionY);
            //PositionY.Subscribe(newPosition => pacmanData.PositionY = newPosition);

            // new
            Vector2 position = new Vector2(pacmanData.PositionX, pacmanData.PositionY);
            NewPosition = new ReactiveProperty<Vector2>(position);

            NewPosition.Subscribe(position =>
            {
                pacmanData.PositionX = position.x;
                pacmanData.PositionY = position.y;
            });

            Direction = new ReactiveProperty<Vector2>(pacmanData.Direction);
            Direction.Subscribe(direction => pacmanData.Direction = new Vector2Int((int)direction.x, (int)direction.y));
        }

        //public ReactiveProperty<float> PositionX;
        //public ReactiveProperty<float> PositionY;
        // new
        public ReactiveProperty<Vector2> NewPosition;   // Вынести в Entity
        public ReactiveProperty<Vector2> Direction;     // Добавить промежуточный класс для персонажей и вынести туда
    }
}
