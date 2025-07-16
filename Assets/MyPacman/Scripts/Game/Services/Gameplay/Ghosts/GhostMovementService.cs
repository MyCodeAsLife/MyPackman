using System;
using System.Collections;
using UnityEngine;

namespace MyPacman
{
    public class GhostMovementService
    {
        private Ghost _entity;
        private Pacman _enemy;
        private Coroutine _moving;
        private TimeService _timeService;
        private GhostBehaviorMode _behaviorMode;

        private Vector2 _targetPosition;
        private Vector2 _mapSize;

        public event Action<GhostMovementService> TargetReached;

        private event Action Moved;

        public GhostMovementService(Ghost entity, Pacman pacman, TimeService timeService, ILevelConfig levelConfig)
        {
            _entity = entity;
            _enemy = pacman;
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

        public void BindBehaviorMode(GhostBehaviorMode behaviorMode)
        {
            if (_behaviorMode == null)
                Moved += Move;

            _behaviorMode = behaviorMode;
        }

        private void Tick()
        {
            Moved?.Invoke();
        }

        private void Move()
        {
            Moved -= Move;
            //----------------------------------------------------------------------------------------------------------
            Vector2 selfPosition = _entity.Position.Value;
            Vector2 selfDirection = _entity.Direction.Value;

            // Добавить функцию подбора модификатора смещения для расчета целевой точки в зависимости от типа призрака.
            // Смещение передовать в GhostBehaviorMode при передаче его в призрака
            if (_behaviorMode.Type == GhostBehaviorModeType.Scatter)
                _targetPosition = new Vector2(29f, 0f);
            else
                _targetPosition = _enemy.Position.Value;
            //----------------------------------------------------------------------------------------------------------
            _entity.Direction.Value =
                    _behaviorMode.CalculateDirectionOfMovement(selfPosition, selfDirection, _targetPosition);

            _moving = Coroutines.StartRoutine(Moving());
        }

        private IEnumerator Moving()
        {
            bool IsMoving = true;
            Vector2 nextPosition = _entity.Position.Value + _entity.Direction.Value.Half();

            while (IsMoving)
            {
                Vector2 currentPosition = _entity.Position.Value;
                float speed = GameConstants.PlayerSpeed * Time.deltaTime;
                Vector2 tempPosition = Vector3.MoveTowards(currentPosition, nextPosition, speed);
                float nextPosX = Utility.RepeatInRange(tempPosition.x, 1, _mapSize.x - 1);
                float nextPosY = Utility.RepeatInRange(tempPosition.y, _mapSize.y + 2, 0);
                _entity.Position.OnNext(new Vector2(nextPosX, nextPosY));

                if (currentPosition == nextPosition || nextPosX != tempPosition.x || nextPosY != tempPosition.y)
                    IsMoving = false;

                yield return null;
            }

            if (_entity.Position.Value == _targetPosition)
                TargetReached?.Invoke(this);

            _moving = null;
            Moved += Move;
        }
    }
}
