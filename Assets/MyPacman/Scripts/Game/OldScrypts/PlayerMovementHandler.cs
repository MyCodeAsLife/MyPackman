using System;
using UnityEngine;

namespace MyPacman
{
    public class PlayerMovementHandler : IPlayerMovementHandler
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Pacman _entity;

        private Func<Vector2> _getDirection;
        private Vector3Int _currentTilePosition;
        private Vector2 _mapSize;
        private Vector2Int _lastDirection;

        public event Action<Vector3Int> TileChanged;

        private event Action Moved;

        public PlayerMovementHandler(Rigidbody2D rigidbody, Pacman entity)
        {
            _rigidbody = rigidbody;
            _entity = entity;
        }

        public void Tick()
        {
            Moved?.Invoke();
        }

        public void Movement()
        {
            Vector2 currentDirection = _getDirection();

            Move(currentDirection);
            Rotate(currentDirection);
        }

        public void StartMoving()
        {
            Moved += Movement;
        }

        public void StopMoving()
        {
            Moved -= Movement;
        }

        //public void Initialyze(Func<Vector2> getDirection, Vector2 mapSize)
        //{
        //    _getDirection = getDirection;
        //    _mapSize = mapSize;

        //    if (_entity != null)
        //    {
        //        //// old
        //        //Vector2 position = new Vector2(_entity.PositionX.CurrentValue, _entity.PositionY.CurrentValue);
        //        //_rigidbody.position = position;

        //        // new
        //        _rigidbody.position = _entity.Position.CurrentValue;
        //    }
        //}

        private void Move(Vector2 currentDirection)
        {
            Vector2 currentPosition = _rigidbody.position;

            float nextPosX = MoveOnAxis(currentPosition.x, currentDirection.x);
            float nextPosY = MoveOnAxis(currentPosition.y, currentDirection.y);

            nextPosX = RepeatInRange(nextPosX, 1, _mapSize.x - 1);
            nextPosY = RepeatInRange(nextPosY, _mapSize.y + 2, 0);

            var newPosition = new Vector2(nextPosX, nextPosY);
            var newTilePosition = Convert.ToTilePosition(newPosition);

            if (_currentTilePosition != newTilePosition)
            {
                _currentTilePosition = newTilePosition;
                TileChanged?.Invoke(_currentTilePosition);
            }

            _rigidbody.position = newPosition;
            _entity.Position.Value = newPosition;
        }

        private void Rotate(Vector2 currentDirection)
        {
            var dir = new Vector2Int((int)currentDirection.x, (int)currentDirection.y);

            if (_lastDirection != dir)
            {
                _entity.Direction.Value = currentDirection;
                _lastDirection = dir;
            }
        }

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

            return nextPosOnAxis;
        }
    }
}