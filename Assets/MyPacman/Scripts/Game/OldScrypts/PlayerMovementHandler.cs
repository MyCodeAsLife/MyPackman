using System;
using UnityEngine;

namespace MyPacman
{
    public class PlayerMovementHandler : IPlayerMovementHandler
    {
        private Rigidbody2D _rigidbody;
        private Func<Vector2> _getDirection;
        //private Func<Vector2, string, bool> _isObstacle;
        private Vector3Int _currentTilePosition;
        private Vector2 _mapSize;

        public event Action<Vector3Int> TileChanged;

        private event Action Moved;

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
            Vector2 currentPosition = _rigidbody.position;

            //currentPosition = HorizontalMove(currentPosition, currentDirection);
            //currentPosition = VerticalMove(currentPosition, currentDirection);

            float nextPosX = MoveOnAxis(currentPosition.x, currentDirection.x);
            float nextPosY = MoveOnAxis(currentPosition.y, currentDirection.y);

            nextPosX = RepeatInRange(nextPosX, 1, _mapSize.x - 1);
            nextPosY = RepeatInRange(nextPosY, _mapSize.y + 2, 0);

            //float xPosInCell = Mathf.Abs(nextPosX - (int)nextPosX);
            //float yPosInCell = Mathf.Abs(nextPosY - (int)nextPosY);
            //var nextCellOnAxisX = new Vector2(currentPosition.x + currentDirection.x, currentPosition.y);
            //var nextCellOnAxisY = new Vector2(currentPosition.x, currentPosition.y + currentDirection.y);

            //if (currentDirection.x != 0)
            //{
            //    if (_isObstacle(nextCellOnAxisX, ""))
            //    {
            //        if (currentDirection.x < -GameConstants.Half && xPosInCell < GameConstants.Half)       // Если двигается влево
            //            nextPosX = GameConstants.Half + (int)nextPosX;
            //        else if (currentDirection.x > GameConstants.Half && xPosInCell > GameConstants.Half)   // Если двигается вправо
            //            nextPosX = GameConstants.Half + (int)nextPosX;
            //    }

            //    nextPosX = RepeatInRange(nextPosX, 1, _mapSize.x - 1);
            //}

            //if (currentDirection.y != 0)
            //{
            //    if (_isObstacle(nextCellOnAxisY, ""))
            //    {
            //        if (currentDirection.y < -GameConstants.Half && yPosInCell > GameConstants.Half)        // Если двигается вниз
            //            nextPosY = -(GameConstants.Half - (int)nextPosY);
            //        else if (currentDirection.y > GameConstants.Half && yPosInCell < GameConstants.Half)    // Если двигается вверх
            //            nextPosY = -(GameConstants.Half - (int)nextPosY);
            //    }

            //    nextPosY = RepeatInRange(nextPosY, _mapSize.y + 2, 0);
            //}
            //----------------------------------------------------------------------------------------------------------
            // Приоритет направления движения у противоположного последнему направлению движения
            // 0. При начале цикла проверить в каком направлении было движение в прошлом цыкле
            // 0.1 Выбрать как приоритет движение по перпендикулярной оси.
            // 1. Проверить можно ли двинутся по выбранной оси, если да то
            // 2.1 Двинутся по выбранной оси
            // 2. Проверить ячейки наискось, если они есть обе то
            // 2.1 Двигать объект в сторону центра его ячейки по указанной оси с половиной скорости
            // 3. Если наискось только одна ячейка то
            // 3.1 Проверить расстояние(вектор) от своего местоположения до непроходимой и от центра своей ячейки по данной оси до непроходимой
            // 3.2 Если расстояние от себя меньше чем от центра, то
            // 3.2.1 Двигать себя в сторону центра по данной оси с половиной скорости
            // 4. Если двинулся то поставить заметку о том что двинулся по данной оси
            // 4.1 Пропустить движение по перпендикулярной оси
            // 5. Проверить было ли движение по предыдуей оси в этом цикле, если да то
            // 5.1 Пропустить движение по данной оси в этом цикле

            var newPosition = new Vector2(nextPosX, nextPosY);
            var newTilePosition = MapHandler.ConvertToCellPosition(newPosition);

            if (_currentTilePosition != newTilePosition)
            {
                _currentTilePosition = newTilePosition;
                TileChanged?.Invoke(_currentTilePosition);
            }

            _rigidbody.position = newPosition;
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
            //_isObstacle = isObstacle;
            _mapSize = mapSize;
        }

        //private Vector2 HorizontalMove(Vector2 currentPosition, Vector2 direction)
        //{
        //    if (direction.x == 0)
        //        return currentPosition;

        //    direction.x = Mathf.Round(direction.x);
        //    float posX = currentPosition.x + (GameConstants.PlayerSpeed * Time.fixedDeltaTime * direction.x);
        //    posX = RepeatInRange(posX, 1, _mapSize.x - 1);
        //    var nextPosition = new Vector3(posX, currentPosition.y, 0);
        //    var nextCell = new Vector2(currentPosition.x + direction.x, currentPosition.y);
        //    float positionInCell = nextPosition.x - (int)nextPosition.x;

        //    if (CheckAisleWidth(currentPosition.y, nextPosition.y) == false)
        //        return currentPosition;

        //    if (_isObstacle(nextCell, $"{direction.x} x Next cell:"))
        //    {
        //        if (direction.x < -GameConstants.Half && positionInCell < GameConstants.Half)       // Если двигается влево
        //            nextPosition.x = GameConstants.Half + (int)nextPosition.x;
        //        else if (direction.x > GameConstants.Half && positionInCell > GameConstants.Half)   // Если двигается вправо
        //            nextPosition.x = GameConstants.Half + (int)nextPosition.x;
        //    }

        //    return nextPosition;
        //}

        //private Vector2 VerticalMove(Vector2 currentPosition, Vector2 direction)
        //{
        //    if (direction.y == 0)
        //        return currentPosition;

        //    direction.y = Mathf.Round(direction.y);
        //    float posY = currentPosition.y + (GameConstants.PlayerSpeed * Time.fixedDeltaTime * direction.y);

        //    posY = RepeatInRange(posY, _mapSize.y + 2, 0);                          // 2 - y offset
        //    var nextPosition = new Vector3(currentPosition.x, posY, 0);
        //    var nextCell = new Vector2(currentPosition.x, currentPosition.y + direction.y);
        //    float positionInCell = Mathf.Abs(nextPosition.y - (int)nextPosition.y);

        //    if (CheckAisleWidth(currentPosition.x, nextPosition.x) == false)
        //        return currentPosition;

        //    if (_isObstacle(nextCell, $"{direction.y} y Next cell:"))
        //    {
        //        if (direction.y < -GameConstants.Half && positionInCell > GameConstants.Half)   // Если двигается вниз
        //            nextPosition.y = -(GameConstants.Half - (int)nextPosition.y);
        //        else if (direction.y > GameConstants.Half && positionInCell < GameConstants.Half)   // Если двигается вверх
        //            nextPosition.y = -(GameConstants.Half - (int)nextPosition.y);
        //    }

        //    return nextPosition;
        //}

        //private bool CheckAisleWidth(float currentPositionOnAxis, float nextPositionOnAxis)     // Перенести в AxisMove ?
        //{
        //    float positionInCell = Mathf.Abs(nextPositionOnAxis - (int)nextPositionOnAxis);

        //    if (positionInCell > 0.3f || positionInCell < 0.7f)     // Ширина просвета для поворота, чем меньше разница тем сложнее свернуть при движении
        //        return true;

        //    return false;
        //}

        private float RepeatInRange(float value, float min, float max)
        {
            if (value < min)
                return max;
            else if (value > max)
                return min;

            return value;
        }

        private float MoveOnAxis(float currentPositionOnAxis, float direction)
        {
            direction = Mathf.Round(direction);
            float nextPosOnAxis = currentPositionOnAxis + (GameConstants.PlayerSpeed * Time.fixedDeltaTime * direction);

            //if (CheckAisleWidth(currentPositionOnAxis, nextPosOnAxis) == false)
            //    return currentPositionOnAxis;

            return nextPosOnAxis;
        }
    }
}