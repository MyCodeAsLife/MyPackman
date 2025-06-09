using System;
using UnityEngine;

namespace MyPacman
{
    public class PlayerMovementHandler : IPlayerMovementHandler
    {
        private Rigidbody2D _rigidbody;
        private Func<Vector2> _getDirection;
        private Func<Vector2, string, bool> _isObstacle;

        private Vector2 _mapSize;

        private event Action Moved;                                  // Вынести в шину событий?

        public PlayerMovementHandler(Rigidbody2D rigidbody)
        {
            _rigidbody = rigidbody;
        }

        public void Tick()
        {
            Moved?.Invoke();
        }

        public void Move()
        {
            Vector2 currentDirection = _getDirection();
            Vector2 nextPosition = _rigidbody.position;

            nextPosition = HorizontalMove(nextPosition, currentDirection);
            nextPosition = VerticalMove(nextPosition, currentDirection);

            _rigidbody.MovePosition(nextPosition);
        }

        public void StartMoving()
        {
            Moved += Move;
        }

        public void StopMoving()
        {
            Moved -= Move;
        }

        public void Initialyze(Func<Vector2> getDirection, Func<Vector2, string, bool> isObstacle, Vector2 mapSize)
        {
            _getDirection = getDirection;
            _isObstacle = isObstacle;
            _mapSize = mapSize;
        }

        private Vector2 HorizontalMove(Vector2 currentPosition, Vector2 direction)
        {
            if (direction.x == 0)
                return currentPosition;

            direction.x = Mathf.Round(direction.x);
            float posX = currentPosition.x + (GameConstants.PlayerSpeed * Time.fixedDeltaTime * direction.x);
            posX = RepeatInRange(posX, 1, _mapSize.x - 1);
            var nextPosition = new Vector3(posX, currentPosition.y, 0);
            var nextCell = new Vector2(currentPosition.x + direction.x, currentPosition.y);
            float positionInCell = nextPosition.x - (int)nextPosition.x;

            if (CheckAisleWidth(currentPosition.y, nextPosition.y) == false)
                return currentPosition;

            if (_isObstacle(nextCell, $"{direction.x} x Next cell:"))
            {
                if (direction.x < -GameConstants.Half && positionInCell < GameConstants.Half)       // Если двигается влево
                    nextPosition.x = GameConstants.Half + (int)nextPosition.x;
                else if (direction.x > GameConstants.Half && positionInCell > GameConstants.Half)   // Если двигается вправо
                    nextPosition.x = GameConstants.Half + (int)nextPosition.x;
            }

            var upCell = new Vector2(nextCell.x, nextCell.y + 1);
            var downCell = new Vector2(nextCell.x, nextCell.y - 1);

            //if (_isObstacle(upCell, "") || _isObstacle(downCell, ""))
            //    nextPosition.y = -(GameConstants.Half - (int)nextPosition.y);

            return nextPosition;
        }

        private Vector2 VerticalMove(Vector2 currentPosition, Vector2 direction)
        {
            if (direction.y == 0)
                return currentPosition;

            direction.y = Mathf.Round(direction.y);
            float posY = currentPosition.y + (GameConstants.PlayerSpeed * Time.fixedDeltaTime * direction.y);

            posY = RepeatInRange(posY, _mapSize.y + 2, 0);                          // 2 - y offset
            var nextPosition = new Vector3(currentPosition.x, posY, 0);
            float positionInCell = Mathf.Abs(nextPosition.y - (int)nextPosition.y);
            var nextCell = new Vector2(currentPosition.x, currentPosition.y + direction.y);

            if (CheckAisleWidth(currentPosition.x, nextPosition.x) == false)
                return currentPosition;

            if (_isObstacle(nextCell, $"{direction.y} y Next cell:"))
            {
                if (direction.y < -GameConstants.Half && positionInCell > GameConstants.Half)   // Если двигается вниз
                    nextPosition.y = -(GameConstants.Half - (int)nextPosition.y);
                else if (direction.y > GameConstants.Half && positionInCell < GameConstants.Half)   // Если двигается вверх
                    nextPosition.y = -(GameConstants.Half - (int)nextPosition.y);
            }

            var leftCell = new Vector2(nextCell.x - 1, nextCell.y);
            var rigthCell = new Vector2(nextCell.x + 1, nextCell.y);

            //if (_isObstacle(leftCell, "") || _isObstacle(rigthCell, ""))
            //    nextPosition.x = GameConstants.Half + (int)nextPosition.x;

            return nextPosition;
        }

        private bool CheckAisleWidth(float currentPositionOnAxis, float nextPositionOnAxis)
        {
            float positionInCell = Mathf.Abs(nextPositionOnAxis - (int)nextPositionOnAxis);

            if (positionInCell > 0.3f || positionInCell < 0.7f)     // Ширина просвета для поворота, чем меньше разница те сложнее свернуть при движении
                return true;

            return false;
        }

        private float RepeatInRange(float value, float min, float max)
        {
            if (value <= min)
                return max;
            else if (value >= max)
                return min;

            return value;
        }
    }
}