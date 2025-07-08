using System;
using System.Collections;
using UnityEngine;

namespace MyPacman
{
    public class GhostMovementService
    {
        private Ghost _entity;
        private TimeService _timeService;
        private IGhostBehaviorMode _behaviorMode;
        private Coroutine _moving;

        private event Action Moved;

        public GhostMovementService(TimeService timeService, Ghost entity)
        {
            _timeService = timeService;
            _entity = entity;

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

            var target = _behaviorMode.CalculatePointOfMovement();
            _moving = Coroutines.StartRoutine(Moving(target));
        }

        private IEnumerator Moving(Vector2 targetPosition)
        {
            bool IsMoving = true;

            while (IsMoving)
            {
                Vector2 currentPosition = _entity.Position.Value;
                Vector2 nextPosition = Vector3.MoveTowards(currentPosition, targetPosition, GameConstants.PlayerSpeed);
                _entity.Position.OnNext(nextPosition);

                if (currentPosition == targetPosition)
                    IsMoving = false;

                yield return null;
            }

            _moving = null;
            Moved += Move;
        }
    }
}
