using System;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace MyPacman
{
    public class PlayerMovementHandler : IPlayerMovementHandler
    {
        private Rigidbody2D _rigidbody;
        private Func<Vector2> _getDirection;
        private Func<Vector2, string, bool> _isObstacle;

        private Vector2 _mapSize;
        private readonly int _obstacleLayer = LayerMask.NameToLayer(GameConstants.Obstacle);

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
            //_rigidbody.MovePosition(currentPosition);

            float nextPosX = MoveOnAxis(currentPosition.x, currentDirection.x);
            float nextPosY = MoveOnAxis(currentPosition.y, currentDirection.y);

            float xPosInCell = Mathf.Abs(nextPosX - (int)nextPosX);
            float yPosInCell = Mathf.Abs(nextPosY - (int)nextPosY);
            var nextCellOnAxisX = new Vector2(currentPosition.x + currentDirection.x, currentPosition.y);
            var nextCellOnAxisY = new Vector2(currentPosition.x, currentPosition.y + currentDirection.y);

            if (currentDirection.x != 0)
            {
                if (_isObstacle(nextCellOnAxisX, ""))
                {
                    if (currentDirection.x < -GameConstants.Half && xPosInCell < GameConstants.Half)       // ���� ��������� �����
                        nextPosX = GameConstants.Half + (int)nextPosX;
                    else if (currentDirection.x > GameConstants.Half && xPosInCell > GameConstants.Half)   // ���� ��������� ������
                        nextPosX = GameConstants.Half + (int)nextPosX;
                }

                nextPosX = RepeatInRange(nextPosX, 1, _mapSize.x - 1);
            }

            if (currentDirection.y != 0)
            {
                if (_isObstacle(nextCellOnAxisY, ""))
                {
                    if (currentDirection.y < -GameConstants.Half && yPosInCell > GameConstants.Half)        // ���� ��������� ����
                        nextPosY = -(GameConstants.Half - (int)nextPosY);
                    else if (currentDirection.y > GameConstants.Half && yPosInCell < GameConstants.Half)    // ���� ��������� �����
                        nextPosY = -(GameConstants.Half - (int)nextPosY);
                }

                nextPosY = RepeatInRange(nextPosY, _mapSize.y + 2, 0);
            }
            // ��������� ����������� �������� � ���������������� ���������� ����������� ��������
            // 0. ��� ������ ����� ��������� � ����� ����������� ���� �������� � ������� �����
            // 0.1 ������� ��� ��������� �������� �� ���������������� ���.
            // 1. ��������� ����� �� �������� �� ��� �, ���� �� ��
            // 2.1 �������� �� ��� �
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

            //----------------------------------------------------------------------------------------------------------
            //if (currentDirection.x != 0 && currentDirection.y != 0)
            //{
            //    //float currentPosXInCell = Mathf.Abs(currentPosition.x - (int)currentPosition.x);
            //    //float currentPosYInCell = Mathf.Abs(currentPosition.y - (int)currentPosition.y);
            //    //float stepOnXAxis = Mathf.Abs(currentPosXInCell - xPosInCell);
            //    //float stepOnYAxis = Mathf.Abs(currentPosYInCell - yPosInCell);

            //    //if (_isObstacle(nextCellOnAxisX, "") == false && _isObstacle(nextCellOnAxisY, "") == false)
            //    //{
            //    //    if (stepOnYAxis < stepOnXAxis)
            //    //    {
            //    //        Debug.Log($"1 - step on x: {stepOnXAxis}. step on y: {stepOnYAxis}");                           // ++++++++++++++++++++++++
            //    //        nextPosY = currentPosition.y;
            //    //    }
            //    //    else if (stepOnXAxis < stepOnYAxis)
            //    //    {
            //    //        Debug.Log($"2 - step on x: {stepOnXAxis}. step on y: {stepOnYAxis}");                           // ++++++++++++++++++++++++
            //    //        nextPosX = currentPosition.x;
            //    //    }
            //    //}

            //    //var nextCell = new Vector2(currentPosition.x + currentDirection.x, currentPosition.y + currentDirection.y);

            //    //if (_isObstacle(nextCell, "") && (_isObstacle(nextCellOnAxisX, "") == false && (_isObstacle(nextCellOnAxisY, "") == false)))
            //    //{
            //    //    float lengthForNextCellX = currentPosition.x + currentDirection.x;
            //    //    float lengthForNextCellY = currentPosition.y + currentDirection.y;

            //    //    if (lengthForNextCellY < lengthForNextCellX)
            //    //    {
            //    //        Debug.Log($"1 - step on x: {lengthForNextCellX}. step on y: {lengthForNextCellY}");                           // ++++++++++++++++++++++++
            //    //        nextPosX = currentPosition.x;
            //    //    }
            //    //    else if (lengthForNextCellX < lengthForNextCellY)
            //    //    {
            //    //        Debug.Log($"2 - step on x: {lengthForNextCellX}. step on y: {lengthForNextCellY}");                           // ++++++++++++++++++++++++
            //    //        nextPosY = currentPosition.y;
            //    //    }
            //    //}

            //}

            _rigidbody.MovePosition(new Vector2(nextPosX, nextPosY));
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
                if (direction.x < -GameConstants.Half && positionInCell < GameConstants.Half)       // ���� ��������� �����
                    nextPosition.x = GameConstants.Half + (int)nextPosition.x;
                else if (direction.x > GameConstants.Half && positionInCell > GameConstants.Half)   // ���� ��������� ������
                    nextPosition.x = GameConstants.Half + (int)nextPosition.x;
            }

            //var upCell = new Vector2(nextCell.x, nextCell.y + 1);
            //var downCell = new Vector2(nextCell.x, nextCell.y - 1);

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
            var nextCell = new Vector2(currentPosition.x, currentPosition.y + direction.y);
            float positionInCell = Mathf.Abs(nextPosition.y - (int)nextPosition.y);

            if (CheckAisleWidth(currentPosition.x, nextPosition.x) == false)
                return currentPosition;

            if (_isObstacle(nextCell, $"{direction.y} y Next cell:"))
            {
                if (direction.y < -GameConstants.Half && positionInCell > GameConstants.Half)   // ���� ��������� ����
                    nextPosition.y = -(GameConstants.Half - (int)nextPosition.y);
                else if (direction.y > GameConstants.Half && positionInCell < GameConstants.Half)   // ���� ��������� �����
                    nextPosition.y = -(GameConstants.Half - (int)nextPosition.y);
            }

            //var leftCell = new Vector2(nextCell.x - 1, nextCell.y);
            //var rigthCell = new Vector2(nextCell.x + 1, nextCell.y);

            //if (_isObstacle(leftCell, "") || _isObstacle(rigthCell, ""))
            //    nextPosition.x = GameConstants.Half + (int)nextPosition.x;

            return nextPosition;
        }

        private bool CheckAisleWidth(float currentPositionOnAxis, float nextPositionOnAxis)     // ��������� � AxisMove ?
        {
            float positionInCell = Mathf.Abs(nextPositionOnAxis - (int)nextPositionOnAxis);

            if (positionInCell > 0.3f || positionInCell < 0.7f)     // ������ �������� ��� ��������, ��� ������ ������� ��� ������� �������� ��� ��������
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

        private float MoveOnAxis(float currentPositionOnAxis, float direction)
        {
            direction = Mathf.Round(direction);
            float nextPosOnAxis = currentPositionOnAxis + (GameConstants.PlayerSpeed * Time.fixedDeltaTime * direction);

            if (CheckAisleWidth(currentPositionOnAxis, nextPosOnAxis) == false)
                return currentPositionOnAxis;

            return nextPosOnAxis;
        }
    }
}