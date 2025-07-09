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
        private IGhostBehaviorMode _behaviorMode;

        private event Action Moved;

        public GhostMovementService(Ghost entity, Pacman pacman, TimeService timeService)
        {
            _entity = entity;
            _enemy = pacman;
            _timeService = timeService;

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

        public void BindBehaviorMode(IGhostBehaviorMode behaviorMode)
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
                Vector2 nextPosition = Vector3.MoveTowards(currentPosition, targetPosition, speed);
                _entity.Position.OnNext(nextPosition);

                //------------------------------------------------
                float nextPosX = MoveOnAxis(currentPosition.x, currentDirection.x);
                float nextPosY = MoveOnAxis(currentPosition.y, currentDirection.y);

                nextPosX = RepeatInRange(nextPosX, 1, _mapSize.x - 1);
                nextPosY = RepeatInRange(nextPosY, _mapSize.y + 2, 0);

                var nextPosition = new Vector2(nextPosX, nextPosY);
                //var newTilePosition = Convert.ToTilePosition(nextPosition);

                _entity.Position.OnNext(nextPosition);
                //-------------------------------------------------------------

                if (currentPosition == targetPosition)
                    IsMoving = false;

                yield return null;
            }

            _moving = null;
            Moved += Move;
        }
    }
}
