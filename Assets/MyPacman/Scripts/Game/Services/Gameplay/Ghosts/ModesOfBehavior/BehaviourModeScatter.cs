using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    // Разбегание
    public class BehaviourModeScatter : GhostBehaviorMode
    {
        private List<Vector2> _gatePositions;

        private Func<List<Vector2>, Dictionary<float, Vector2>> CalculateDirections;

        public BehaviourModeScatter(MapHandlerService mapHandlerService, Ghost self, Vector2 targetPosition)
            : base(mapHandlerService, self, GhostBehaviorModeType.Scatter)
        {
            _targetPosition.OnNext(targetPosition);
            _gatePositions = mapHandlerService.GetTilePositions(GameConstants.GateTile);  // Таким же макаром получать позицию своего спавна?
            CalculateDirections = CalculateDirectionsToGatePosition;
        }

        protected override Vector2 CalculateDirection()
        {
            var availableDirections = _mapHandlerService.GetDirectionsWithoutWalls(_selfPosition);

            if (availableDirections.Count == 1)
                return -_selfDirection;
            else if (availableDirections.Count == 2)
                return availableDirections.First(value => value != -_selfDirection);
            else if (_mapHandlerService.CheckTileForObstacle(_selfPosition))
                return _selfDirection;

            return CalculateDirectionInSelectedMode(availableDirections);
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)  // Похожа на такуюже в BehaviourModeFrightened
        {
            Dictionary<float, Vector2> directionsMap = CalculateDirections(availableDirections);
            Vector2 direction = SelectRandomDirection(directionsMap);
            return direction;
        }

        private Dictionary<float, Vector2> CalculateDirectionsToScatterPosition(List<Vector2> availableDirections)
        {
            var calculateDirections = CreateDirectionsMap(availableDirections, _targetPosition.Value);
            return RemoveUnnecessaryDirection(calculateDirections);
        }

        private Dictionary<float, Vector2> CalculateDirectionsToGatePosition(List<Vector2> availableDirections)
        {
            CheckNeighboringTilesForGates();
            Vector2 targetPosition = CalculatePositionNearestGate();
            var calculateDirections = CreateDirectionsMap(availableDirections, targetPosition);
            return RemoveUnnecessaryDirection(calculateDirections);
        }

        private void CheckNeighboringTilesForGates()
        {
            var lastPosition = _selfPosition + -_selfDirection;

            if (_mapHandlerService.CheckTile(lastPosition, GameConstants.GateTile))
                CalculateDirections = CalculateDirectionsToScatterPosition;
        }

        private Dictionary<float, Vector2> RemoveUnnecessaryDirection(Dictionary<float, Vector2> calculateDirections)
        {
            while (calculateDirections.Count > 1)   // Именно 1, потому как движение в обратную сторону уже убранно
            {
                float maxDistance = float.MinValue;

                foreach (var direction in calculateDirections)
                {
                    if (direction.Key > maxDistance)
                        maxDistance = direction.Key;
                }

                calculateDirections.Remove(calculateDirections.First(value => value.Key == maxDistance).Key);
            }

            return calculateDirections;
        }

        private Vector2 CalculatePositionNearestGate()
        {
            Vector2 targetPosition = _gatePositions.First();
            float minDistance = targetPosition.SqrDistance(_selfPosition);

            foreach (var gatePosition in _gatePositions)
            {
                float distance = gatePosition.SqrDistance(_selfPosition);

                if (distance < minDistance)
                    targetPosition = gatePosition;
            }

            return targetPosition;
        }

        private Dictionary<float, Vector2> CreateDirectionsMap(List<Vector2> directions, Vector2 targetPosition)
        {
            //float maxDistance = float.MinValue;
            Dictionary<float, Vector2> directionsMap = new();

            foreach (var direction in directions)
            {
                if (direction == -_selfDirection)
                    continue;

                float distance = targetPosition.SqrDistance(_selfPosition + direction);
                directionsMap[distance] = direction;

                //if (distance > maxDistance)
                //    maxDistance = distance;
            }

            //if (directionsMap.Count > 2)
            //    directionsMap.Remove(directionsMap.First(value => value.Key == maxDistance).Key);

            return directionsMap;
        }
    }
}