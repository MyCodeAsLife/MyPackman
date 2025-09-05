using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    public abstract class GhostBehaviorMode
    {
        protected readonly MapHandlerService _mapHandlerService;
        protected readonly Ghost _self;
        protected readonly IReadOnlyList<Vector2> _speedModifierPositions;
        protected readonly ReactiveProperty<Vector2> _targetPosition = new();

        protected Vector2 _selfPosition;                // Для кеширования _self.Position?
        protected Vector2 _selfDirection;

        public GhostBehaviorMode(MapHandlerService mapHandlerService, Ghost self, GhostBehaviorModeType behaviorModeType)
        {
            _mapHandlerService = mapHandlerService;
            _self = self;
            Type = behaviorModeType;

            _speedModifierPositions = mapHandlerService.SpeedModifierPositions;
        }

        public ReadOnlyReactiveProperty<Vector2> TargetPosition => _targetPosition;
        public GhostBehaviorModeType Type { get; private set; }

        public Vector2 CalculateDirectionOfMovement()
        {
            _selfPosition = _self.Position.Value;
            _selfDirection = _self.Direction.Value;

            if (_mapHandlerService.IsCenterTail(_selfPosition) == false)
                return _selfDirection;

            return CalculateDirection();
        }

        public void CheckSurfaceModifier()      // Вынести в GhostsStateHandler ?
        {
            foreach (var modifierPosition in _speedModifierPositions)
            {
                if (modifierPosition == _selfPosition)
                {
                    if (_self.SpeedModifier.Value == GameConstants.GhostTunelSpeedModifier)
                        _self.SpeedModifier.Value = GameConstants.GhostNormalSpeed​​Modifier;
                    else
                        _self.SpeedModifier.Value = GameConstants.GhostTunelSpeedModifier;
                }
            }
        }

        protected abstract Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections);

        protected virtual Vector2 CalculateDirection(List<Vector2> availableDirections = null)
        {
            if (availableDirections == null)
                availableDirections = _mapHandlerService.GetDirectionsWithoutObstacles(_selfPosition);

            if (availableDirections.Count == 1)
                return -_selfDirection;
            else if (availableDirections.Count == 2)
                return availableDirections.First(value => value != -_selfDirection);
            else if (_mapHandlerService.CheckTileForObstacle(_selfPosition))
                return _selfDirection;

            return CalculateDirectionInSelectedMode(availableDirections);
        }

        protected Vector2 SelectRandomDirection(Dictionary<float, Vector2> directionsMap)
        {
            Vector2 direction = Vector2.zero;
            int rand = UnityEngine.Random.Range(0, directionsMap.Count);
            int counter = 0;

            foreach (var selectDirection in directionsMap)
            {
                if (rand == counter)
                {
                    direction = selectDirection.Value;
                    break;
                }

                counter++;
            }

            return direction;
        }

        // Создает карту всех направлений с показанием веса(расстояние от выбранного направления до целевой точки)
        // исключая направление назад
        protected Dictionary<float, Vector2> CalculateDirectionsClosestToTarget(List<Vector2> directions, Vector2 targetPosition)
        {
            Dictionary<float, Vector2> directionsMap = new();

            foreach (var direction in directions)
            {
                if (direction == -_selfDirection)
                    continue;

                float distance = targetPosition.SqrDistance(_selfPosition + direction);
                directionsMap[distance] = direction;
            }

            return directionsMap;
        }

        protected Dictionary<float, Vector2> RemoveWrongDirection(
            Dictionary<float, Vector2> calculateDirections,
            Func<float, float, bool> compare)
        {
            // Направление назад уже убранно
            while (calculateDirections.Count > 2)
            {
                float wrongDistance = 0;

                foreach (var direction in calculateDirections)
                    if (compare(direction.Key, wrongDistance))
                        wrongDistance = direction.Key;

                calculateDirections.Remove(wrongDistance);
            }

            return calculateDirections;
        }

        protected bool ItFar(float value1, float value2) => value1 > value2;
        protected bool ItNear(float value1, float value2) => value1 < value2;
    }
}