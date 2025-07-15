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
        private Vector2 _mapSize;

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

        private void Tick()
        {
            Moved?.Invoke();
        }

        public void BindBehaviorMode(GhostBehaviorMode behaviorMode)
        {
            if (_behaviorMode == null)
            {
                _behaviorMode = behaviorMode;
                Moved += Move;
            }
            else
            {
                _behaviorMode = behaviorMode;
            }
        }

        private void Move()
        {
            Moved -= Move;

            var selfPosition = _entity.Position.Value;
            var selfDirection = _entity.Direction.Value;
            var enemyPosition = _enemy.Position.Value;

            _entity.Direction.Value =
                _behaviorMode.CalculateDirectionOfMovement(selfPosition, selfDirection, enemyPosition);

            _moving = Coroutines.StartRoutine(Moving());
        }

        private IEnumerator Moving()
        {
            bool IsMoving = true;
            Vector2 targetPosition = _entity.Position.Value + _entity.Direction.Value.Half();

            while (IsMoving)
            {
                Vector2 currentPosition = _entity.Position.Value;
                float speed = GameConstants.PlayerSpeed * Time.deltaTime;
                Vector2 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, speed);
                float nextPosX = Utility.RepeatInRange(newPosition.x, 1, _mapSize.x - 1);
                float nextPosY = Utility.RepeatInRange(newPosition.y, _mapSize.y + 2, 0);
                var nextPosition = new Vector2(nextPosX, nextPosY);
                _entity.Position.OnNext(nextPosition);

                if (currentPosition == targetPosition || nextPosX != newPosition.x || nextPosY != newPosition.y)
                    IsMoving = false;

                yield return null;
            }

            _moving = null;
            Moved += Move;
        }
    }
}
