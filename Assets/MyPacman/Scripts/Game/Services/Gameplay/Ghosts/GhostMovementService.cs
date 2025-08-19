using R3;
using System;
using System.Collections;
using UnityEngine;

namespace MyPacman
{
    public class GhostMovementService
    {
        private Ghost _entity;
        private Coroutine _moving;
        private TimeService _timeService;
        private GhostBehaviorMode _behaviorMode;

        private Vector2 _targetPosition;
        private Vector2 _mapSize;

        public event Action<EntityType> TargetReached;

        private event Action Moved;

        public GhostMovementService(Ghost entity, ReadOnlyReactiveProperty<Vector2> pacmanPosition, TimeService timeService, ILevelConfig levelConfig)
        {
            _entity = entity;
            _timeService = timeService;
            _mapSize = new Vector2(levelConfig.Map.GetLength(1), -levelConfig.Map.GetLength(0));    //Передать сюда только вектор с размером карты

            _timeService.TimeHasTicked += Tick;
        }

        ~GhostMovementService()
        {
            _timeService.TimeHasTicked -= Tick;
            Coroutines.StopRoutine(_moving);
        }

        public GhostBehaviorModeType BehaviorModeType => _behaviorMode.Type;
        public EntityType EntityType => _entity.Type;

        public void BindBehaviorMode(GhostBehaviorMode behaviorMode)
        {
            if (_behaviorMode == null)
                Moved += Move;

            _behaviorMode = behaviorMode;
            _behaviorMode._targetPosition.Subscribe(newPos => _targetPosition = newPos);
        }

        private void Tick()
        {
            Moved?.Invoke();
        }

        private void Move()
        {
            Moved -= Move;
            _entity.Direction.Value = _behaviorMode.CalculateDirectionOfMovement();
            _moving = Coroutines.StartRoutine(Moving());
        }

        private IEnumerator Moving()
        {
            bool IsMoving = true;
            Vector2 nextPosition = _entity.Position.Value + _entity.Direction.Value.Half();

            while (IsMoving)
            {
                Vector2 currentPosition = _entity.Position.Value;
                float speed = GameConstants.PlayerSpeed * _timeService.DeltaTime;
                Vector2 tempPosition = Vector3.MoveTowards(currentPosition, nextPosition, speed);
                float nextPosX = Utility.RepeatInRange(tempPosition.x, 1, _mapSize.x - 1);
                float nextPosY = Utility.RepeatInRange(tempPosition.y, _mapSize.y + 2, 0);
                _entity.Position.OnNext(new Vector2(nextPosX, nextPosY));

                if (GreaterThanOrEqual(currentPosition, nextPosition, _entity.Direction.Value)
                    || nextPosX != tempPosition.x
                    || nextPosY != tempPosition.y
                    )
                    IsMoving = false;

                yield return null;
            }

            // Возможное место фиксации столкновения с игроком
            //if (_entity.Position.Value.SqrDistance(_targetPosition) < 1f)       // Если расстояние до цели меньше размера тайла
            if (_entity.Position.Value == _targetPosition)
                TargetReached?.Invoke(_entity.Type);

            _moving = null;
            Moved += Move;
        }

        private bool GreaterThanOrEqual(Vector2 firstPos, Vector2 secondPos, Vector2 direction)
        {
            if (firstPos == secondPos)
                return true;

            if (direction.x != 0)
            {
                if (direction.x > 0)
                    return firstPos.x > secondPos.x;
                else
                    return firstPos.x < secondPos.x;
            }
            else if (direction.y != 0)
            {
                if (direction.y > 0)
                    return firstPos.y > secondPos.y;
                else
                    return firstPos.y < secondPos.y;
            }

            throw new Exception(                                                    // Magic
                $"Unknown error." +
                $"First pos: {firstPos}." +
                $"Second pos: {secondPos}." +
                $"Direction: {direction}");
        }
    }
}