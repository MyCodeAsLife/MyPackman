using System;
using UnityEngine;

namespace MyPacman
{
    public class PlayerMovementHandler : IPlayerMovementHandler
    {
        private Rigidbody2D _rigidbody;
        private Func<Vector2> _getDirection;
        private Func<Vector2, string, bool> _isObstacle;

        private Vector2 _lastDirectionMove = new();
        private Vector2 _lastPosition = new();

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

            //if (_lastDirectionMove.y == 0)
            nextPosition = HorizontalMove(nextPosition, currentDirection);
            //else
            //    _lastDirectionMove.y = 0;

            //if (_lastDirectionMove.x == 0)
            nextPosition = VerticalMove(nextPosition, currentDirection);
            //else
            //    _lastDirectionMove.x = 0;


            _lastPosition = nextPosition;
            // Если одна из осей в ячейке не равна 0.5, проверить соседние ячейки по этой оси на препятствия
            // если препятствия есть, то по данной оси откатится к 0.5
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

        public void Initialyze(Func<Vector2> getDirection, Func<Vector2, string, bool> isObstacle)
        {
            _getDirection = getDirection;
            _isObstacle = isObstacle;
        }

        private Vector2 HorizontalMove(Vector2 currentPosition, Vector2 direction)
        {
            if (direction.x == 0)
                return currentPosition;

            direction.x = Mathf.Round(direction.x);
            float posX = currentPosition.x + (GameConstants.PlayerSpeed * Time.fixedDeltaTime * direction.x);
            var nextPosition = new Vector3(posX, currentPosition.y, 0);
            var nextCell = new Vector2(currentPosition.x + direction.x, currentPosition.y);
            float positionInCell = nextPosition.x - (int)nextPosition.x;

            var upCell = new Vector2(nextCell.x, nextCell.y + 1);
            var downCell = new Vector2(nextCell.x, nextCell.y - 1);

            float positionInCellOnAxisY = Mathf.Abs(nextPosition.y - (int)nextPosition.y);

            if (positionInCellOnAxisY < 0.3f || positionInCellOnAxisY > 0.7f)     // Ширина просвета для поворота, чем меньше разница те сложнее свернуть при движении
                return currentPosition;

            if (_isObstacle(upCell, "") || _isObstacle(downCell, ""))
            {


                if (direction.x < -GameConstants.Half)       // Если двигается влево
                {
                    if (positionInCell < GameConstants.Half)
                        if (_isObstacle(nextCell, "-1 x Next cell:"))
                            nextPosition.x = GameConstants.Half + (int)nextPosition.x;
                }
                else if (direction.x > GameConstants.Half)   // Если двигается вправо
                {
                    if (positionInCell > GameConstants.Half)
                        if (_isObstacle(nextCell, "1 x Next cell:"))
                            nextPosition.x = GameConstants.Half + (int)nextPosition.x;
                }
            }

            return nextPosition;
        }

        private Vector2 VerticalMove(Vector2 currentPosition, Vector2 direction)
        {
            if (direction.y == 0)
                return currentPosition;

            direction.y = Mathf.Round(direction.y);
            float posY = currentPosition.y + (GameConstants.PlayerSpeed * Time.fixedDeltaTime * direction.y);
            var nextPosition = new Vector3(currentPosition.x, posY, 0);
            float positionInCell = Mathf.Abs(nextPosition.y - (int)nextPosition.y);
            var nextCell = new Vector2(currentPosition.x, currentPosition.y + direction.y);

            var leftCell = new Vector2(nextCell.x - 1, nextCell.y);
            var rigthCell = new Vector2(nextCell.x + 1, nextCell.y);

            float positionInCellOnAxisX = nextPosition.x - (int)nextPosition.x;

            if (positionInCellOnAxisX < 0.3f || positionInCellOnAxisX > 0.7f)
                return currentPosition;

            if (_isObstacle(leftCell, "") || _isObstacle(rigthCell, ""))
            {
                if (direction.y < -GameConstants.Half)   // Если двигается вниз
                {
                    if (positionInCell > GameConstants.Half)
                        if (_isObstacle(nextCell, "-1 y Next cell:"))
                            nextPosition.y = -(GameConstants.Half - (int)nextPosition.y);
                }
                else if (direction.y > GameConstants.Half)   // Если двигается вверх
                {
                    if (positionInCell < GameConstants.Half)
                        if (_isObstacle(nextCell, "1 y Next cell:"))
                            nextPosition.y = -(GameConstants.Half - (int)nextPosition.y);
                }


            }
            return nextPosition;
        }
    }
}