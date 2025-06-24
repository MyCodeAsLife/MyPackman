using System;
using UnityEngine;

namespace MyPacman
{
    public class PlayerMovementHandler : IPlayerMovementHandler
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly PacmanEntity _entity;
        private Func<Vector2> _getDirection;
        private Vector3Int _currentTilePosition;
        private Vector2 _mapSize;

        public event Action<Vector3Int> TileChanged;

        private event Action Moved;

        public PlayerMovementHandler(Rigidbody2D rigidbody, PacmanEntity entity)
        {
            _rigidbody = rigidbody;
            _entity = entity;
        }

        public void Tick()
        {
            Moved?.Invoke();
        }

        public void Move()
        {
            Vector2 currentDirection = _getDirection();
            Vector2 currentPosition = _rigidbody.position;

            float nextPosX = MoveOnAxis(currentPosition.x, currentDirection.x);
            float nextPosY = MoveOnAxis(currentPosition.y, currentDirection.y);

            nextPosX = RepeatInRange(nextPosX, 1, _mapSize.x - 1);
            nextPosY = RepeatInRange(nextPosY, _mapSize.y + 2, 0);

            var newPosition = new Vector2(nextPosX, nextPosY);
            var newTilePosition = MapHandler.ConvertToTilePosition(newPosition);

            if (_currentTilePosition != newTilePosition)
            {
                _currentTilePosition = newTilePosition;
                TileChanged?.Invoke(_currentTilePosition);
            }

            _rigidbody.position = newPosition;

            if (_entity != null)        // Переделать под реактивщину
            {
                _entity.Position.Value = newTilePosition;

                //// old
                //_entity.PositionX.Value = newPosition.x;
                //_entity.PositionY.Value = newPosition.y;

                // new
                _entity.NewPosition.Value = newPosition;
                _entity.Direction.Value = currentDirection;
            }
        }

        public void StartMoving()
        {
            Moved += Move;
        }

        public void StopMoving()
        {
            Moved -= Move;
        }

        public void Initialyze(Func<Vector2> getDirection, Vector2 mapSize)
        {
            _getDirection = getDirection;
            _mapSize = mapSize;

            if (_entity != null)
            {
                //// old
                //Vector2 position = new Vector2(_entity.PositionX.CurrentValue, _entity.PositionY.CurrentValue);
                //_rigidbody.position = position;

                // new
                _rigidbody.position = _entity.NewPosition.CurrentValue;
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