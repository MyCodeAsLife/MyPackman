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
            //        if (currentDirection.x < -GameConstants.Half && xPosInCell < GameConstants.Half)       // ���� ��������� �����
            //            nextPosX = GameConstants.Half + (int)nextPosX;
            //        else if (currentDirection.x > GameConstants.Half && xPosInCell > GameConstants.Half)   // ���� ��������� ������
            //            nextPosX = GameConstants.Half + (int)nextPosX;
            //    }

            //    nextPosX = RepeatInRange(nextPosX, 1, _mapSize.x - 1);
            //}

            //if (currentDirection.y != 0)
            //{
            //    if (_isObstacle(nextCellOnAxisY, ""))
            //    {
            //        if (currentDirection.y < -GameConstants.Half && yPosInCell > GameConstants.Half)        // ���� ��������� ����
            //            nextPosY = -(GameConstants.Half - (int)nextPosY);
            //        else if (currentDirection.y > GameConstants.Half && yPosInCell < GameConstants.Half)    // ���� ��������� �����
            //            nextPosY = -(GameConstants.Half - (int)nextPosY);
            //    }

            //    nextPosY = RepeatInRange(nextPosY, _mapSize.y + 2, 0);
            //}
            //----------------------------------------------------------------------------------------------------------
            // ��������� ����������� �������� � ���������������� ���������� ����������� ��������
            // 0. ��� ������ ����� ��������� � ����� ����������� ���� �������� � ������� �����
            // 0.1 ������� ��� ��������� �������� �� ���������������� ���.
            // 1. ��������� ����� �� �������� �� ��������� ���, ���� �� ��
            // 2.1 �������� �� ��������� ���
            // 2. ��������� ������ ��������, ���� ��� ���� ��� ��
            // 2.1 ������� ������ � ������� ������ ��� ������ �� ��������� ��� � ��������� ��������
            // 3. ���� �������� ������ ���� ������ ��
            // 3.1 ��������� ����������(������) �� ������ �������������� �� ������������ � �� ������ ����� ������ �� ������ ��� �� ������������
            // 3.2 ���� ���������� �� ���� ������ ��� �� ������, ��
            // 3.2.1 ������� ���� � ������� ������ �� ������ ��� � ��������� ��������
            // 4. ���� �������� �� ��������� ������� � ��� ��� �������� �� ������ ���
            // 4.1 ���������� �������� �� ���������������� ���
            // 5. ��������� ���� �� �������� �� ��������� ��� � ���� �����, ���� �� ��
            // 5.1 ���������� �������� �� ������ ��� � ���� �����

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
        //        if (direction.x < -GameConstants.Half && positionInCell < GameConstants.Half)       // ���� ��������� �����
        //            nextPosition.x = GameConstants.Half + (int)nextPosition.x;
        //        else if (direction.x > GameConstants.Half && positionInCell > GameConstants.Half)   // ���� ��������� ������
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
        //        if (direction.y < -GameConstants.Half && positionInCell > GameConstants.Half)   // ���� ��������� ����
        //            nextPosition.y = -(GameConstants.Half - (int)nextPosition.y);
        //        else if (direction.y > GameConstants.Half && positionInCell < GameConstants.Half)   // ���� ��������� �����
        //            nextPosition.y = -(GameConstants.Half - (int)nextPosition.y);
        //    }

        //    return nextPosition;
        //}

        //private bool CheckAisleWidth(float currentPositionOnAxis, float nextPositionOnAxis)     // ��������� � AxisMove ?
        //{
        //    float positionInCell = Mathf.Abs(nextPositionOnAxis - (int)nextPositionOnAxis);

        //    if (positionInCell > 0.3f || positionInCell < 0.7f)     // ������ �������� ��� ��������, ��� ������ ������� ��� ������� �������� ��� ��������
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