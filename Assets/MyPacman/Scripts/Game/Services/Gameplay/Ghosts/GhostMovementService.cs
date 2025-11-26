using R3;
using System;
using System.Collections;
using UnityEngine;

namespace MyPacman
{
    public class GhostMovementService
    {
        private readonly Ghost _entity;
        private readonly TimeService _timeService;
        private readonly Vector2 _mapSize;

        private Coroutine _moving;
        private GhostBehaviorMode _behaviorMode;
        private Vector2 _targetPosition;

        private Action MoveAccordingSelectedAlgorithm;     //Движение со стартовой проверкой и без

        public event Action<EntityType> TargetReached;

        private event Action Moved;

        public GhostMovementService(
            Ghost entity,
            ReadOnlyReactiveProperty<Vector2> pacmanPosition,
            TimeService timeService,
            ILevelConfig levelConfig)
        {
            _entity = entity;
            _timeService = timeService;
            _mapSize = new Vector2(levelConfig.Map.GetLength(1), -levelConfig.Map.GetLength(0));    //Передать сюда только вектор с размером карты

            _timeService.TimeHasTicked += Tick;
            MoveAccordingSelectedAlgorithm = MoveWithCorrection;
        }

        ~GhostMovementService()
        {
            _timeService.TimeHasTicked -= Tick;
            Coroutines.StopRoutine(_moving);
        }

        public GhostBehaviorModeType BehaviorModeType => _behaviorMode.Type;

        public void BindBehaviorMode(GhostBehaviorMode behaviorMode)
        {
            Debug.Log($"{_entity.Type} bind {behaviorMode.Type}");      //+++++++++++++++++++++++++++++++

            if (_behaviorMode == null)
                Moved += Move;

            _behaviorMode = behaviorMode;
            _entity.CurrentBehaviorMode.Value = behaviorMode.Type;
            _behaviorMode.TargetPosition.Subscribe(newPos => _targetPosition = newPos);
            ChangeSpeedModifier(behaviorMode.Type);
        }

        private void ChangeSpeedModifier(GhostBehaviorModeType behaviorType)
        {
            if (behaviorType == GhostBehaviorModeType.Scatter
                && _entity.SpeedModifier.CurrentValue == GameConstants.GhostHomecommingSpeedModifier)
            {
                _entity.SpeedModifier.Value = GameConstants.GhostNormalSpeedModifier;
            }
            else if (behaviorType == GhostBehaviorModeType.Homecomming)
            {
                _entity.SpeedModifier.Value = GameConstants.GhostHomecommingSpeedModifier;
            }
        }

        private void Tick()
        {
            Moved?.Invoke();
            CheckPosition(_targetPosition);
        }

        private void Move()
        {
            Moved -= Move;
            _entity.Direction.Value = _behaviorMode.CalculateDirectionOfMovement();
            MoveAccordingSelectedAlgorithm();
        }

        private void MoveWithoutCorrection()
        {
            Vector2 nextPosition = _entity.Position.Value + _entity.Direction.Value.Half();
            _moving = Coroutines.StartRoutine(Moving(nextPosition));
        }

        private void MoveWithCorrection()
        {
            MoveAccordingSelectedAlgorithm = MoveWithoutCorrection;
            Vector2 startPos = _entity.Position.Value;
            startPos.x = startPos.x - (int)startPos.x;
            startPos.y = startPos.y - (int)startPos.y;

            if ((startPos.x == 0 || startPos.x == 0.5f) && (startPos.y == 0 || startPos.y == 0.5f))
            {
                MoveWithoutCorrection();
                return;
            }

            Vector2 nextPosition = InitialPositionCalculation();
            _moving = Coroutines.StartRoutine(Moving(nextPosition));
        }

        private Vector2 InitialPositionCalculation()
        {
            Vector2 targetPosition = _entity.Position.Value;

            if (_entity.Direction.Value.y != 0)
                targetPosition.y = AxisAndPositionCorrection(targetPosition.y, _entity.Direction.Value.y);
            else
                targetPosition.x = AxisAndPositionCorrection(targetPosition.x, _entity.Direction.Value.x);

            return targetPosition;
        }

        private float AxisAndPositionCorrection(float startPos, float axis)
        {
            float targetPositionOnAxis;

            if (startPos < 0)
            {
                startPos *= -1;
                axis *= -1;
                targetPositionOnAxis = CorrectionTargetPosition(startPos, axis);
                targetPositionOnAxis *= -1;
            }
            else
            {
                targetPositionOnAxis = CorrectionTargetPosition(startPos, axis);
            }

            return targetPositionOnAxis;
        }

        private float CorrectionTargetPosition(float startPos, float axis)
        {
            float offset = startPos - (int)startPos;

            if (axis > 0)
                offset = offset > 0.5f ? 1f : 0.5f;         // Magic        Half direction(movement step)
            else
                offset = offset > 0.5f ? 0.5f : 0f;         // Magic        Half direction(movement step)

            return (int)startPos + offset;
        }

        private void CheckPosition(Vector2 target)
        {
            if (target.SqrDistance(_entity.Position.CurrentValue) < 1.2f)// Magic (расстояние между призраком и целевой точкой)
                TargetReached?.Invoke(_entity.Type);
        }

        private IEnumerator Moving(Vector2 nextPosition)
        {
            bool IsMoving = true;

            while (IsMoving)
            {
                // Calculate movement
                float speed = GameConstants.GhostSpeed * _entity.SpeedModifier.Value * _timeService.DeltaTime;
                Vector2 tempPosition = Vector3.MoveTowards(_entity.Position.Value, nextPosition, speed);
                float nextPosX = Utility.RepeatInRange(tempPosition.x, 1, _mapSize.x - 1);
                float nextPosY = Utility.RepeatInRange(tempPosition.y, _mapSize.y + 2, 0);
                _entity.Position.OnNext(new Vector2(nextPosX, nextPosY));

                // Check state
                if (Utility.GreaterThanOrEqual(_entity.Position.Value, nextPosition, _entity.Direction.Value)
                    || nextPosX != tempPosition.x
                    || nextPosY != tempPosition.y
                    )
                    IsMoving = false;

                yield return null;
            }

            _moving = null;
            Moved += Move;
        }
    }
}