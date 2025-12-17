using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    // Переименовать в класс простого движения, "кротчайшим" путем к указанной точке
    public class GhostBehaviorMode
    {
        // Подкорректировать именование переменных (с большой буквы protect?)
        protected readonly PickableEntityHandler _mapHandlerService;
        protected readonly Ghost _self;
        protected readonly IReadOnlyList<Vector2> _speedModifierPositions;
        protected readonly ReactiveProperty<Vector2> _targetPosition = new();

        private Vector2 _selfPosition;                // Для кеширования _self.Position?
        private Vector2 _selfDirection;

        public GhostBehaviorMode(PickableEntityHandler mapHandlerService, Ghost self, GhostBehaviorModeType behaviorModeType)
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
            CheckSurfaceModifier();

            if (_mapHandlerService.IsCenterOfTile(_selfPosition) == false)
                return _selfDirection;

            return CalculateDirection();
        }

        protected virtual Vector2 CalculateDirection(List<Vector2> availableDirections = null)
        {
            if (availableDirections == null)
                availableDirections = _mapHandlerService.GetDirectionsWithoutObstacles(_selfPosition);

            if (availableDirections.Count == 1)
                return -_selfDirection;
            else if (_mapHandlerService.CheckTileForObstacle(_selfPosition))
                return _selfDirection;

            return CalculateDirectionInSelectedMode(availableDirections);
        }

        protected virtual Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)
        {
            availableDirections = RemoveReverseDirection(availableDirections);
            var directionsMap = CalculateDirectionsMap(availableDirections, _targetPosition.Value);
            directionsMap = RemoveWrongDirection(directionsMap, ItFar);
            return SelectNearestDirection(directionsMap);
        }

        protected Dictionary<float, Vector2> CalculateDirectionsMap(List<Vector2> directions, Vector2 targetPosition)
        {
            Dictionary<float, Vector2> directionsMap = new();

            foreach (var direction in directions)
            {
                float distance = targetPosition.SqrDistance(_selfPosition + direction);
                directionsMap[distance] = direction;
            }

            return directionsMap;
        }

        protected List<Vector2> RemoveReverseDirection(List<Vector2> directions)
        {
            foreach (var direction in directions)
            {
                if (direction == -_selfDirection)     // Убирает направление движения назад
                {
                    directions.Remove(direction);
                    return directions;
                }
            }

            return directions;
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

        protected Vector2 SelectNearestDirection(Dictionary<float, Vector2> directionsMap)
        {
            float minDistance = float.MaxValue;

            foreach (var direction in directionsMap)
                if (direction.Key < minDistance)
                    minDistance = direction.Key;

            return directionsMap[minDistance];
        }

        // Переименовать.
        protected Dictionary<float, Vector2> RemoveWrongDirection(  // Удаляет направление, наименее удовлетворяющее "compare"
            Dictionary<float, Vector2> calculateDirections,
            Func<float, float, bool> compare)
        {
            if (calculateDirections.Count > 1)
            {
                float wrongDistance = calculateDirections.First().Key;

                foreach (var direction in calculateDirections)
                    if (compare(direction.Key, wrongDistance))
                        wrongDistance = direction.Key;

                calculateDirections.Remove(wrongDistance);
            }

            return calculateDirections;
        }

        protected bool ItFar(float value1, float value2) => value1 > value2;
        protected bool ItNear(float value1, float value2) => value1 < value2;

        private void CheckSurfaceModifier()      // Вынести в MovementService ?
        {
            foreach (var modifierPosition in _speedModifierPositions)
            {
                if (modifierPosition == _selfPosition &&
                    _self.SpeedModifier.Value != GameConstants.GhostHomecommingSpeedModifier)
                {
                    if (_self.SpeedModifier.Value == GameConstants.GhostTunelSpeedModifier)
                        _self.SpeedModifier.Value = GameConstants.NormalSpeed​​Modifier;
                    else
                        _self.SpeedModifier.Value = GameConstants.GhostTunelSpeedModifier;
                }
            }
        }
    }
}