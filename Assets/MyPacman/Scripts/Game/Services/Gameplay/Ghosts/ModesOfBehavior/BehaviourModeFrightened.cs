using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    public class BehaviourModeFrightened : GhostBehaviorMode
    {
        public BehaviourModeFrightened(MapHandlerService mapHandlerService)
            : base(mapHandlerService, GhostBehaviorModeType.Frightened) { }

        protected override Vector2 CalculateDirection()
        {
            var availableDirections = _mapHandlerService.GetDirectionsWithoutObstacles(_selfPosition);

            if (availableDirections.Count == 1)
                return -_selfDirection;
            else if (availableDirections.Count == 2)
                return availableDirections.First(value => value != -_selfDirection);

            Dictionary<float, Vector2> directionsMap = CreateDirectionsMap(availableDirections);
            return SelectRandomDirection(directionsMap);
        }

        private Dictionary<float, Vector2> CreateDirectionsMap(List<Vector2> availableDirections)
        {
            float minDistance = float.MaxValue;
            Dictionary<float, Vector2> directionsMap = new();

            foreach (var direction in availableDirections)
            {
                if (direction == -_selfDirection)
                    continue;

                float distance = _enemyPosition.SqrDistance(_selfPosition + direction);
                directionsMap[distance] = direction;

                if (distance < minDistance)
                    minDistance = distance;
            }

            directionsMap.Remove(directionsMap.First(value => value.Key == minDistance).Key);
            return directionsMap;
        }
    }
}