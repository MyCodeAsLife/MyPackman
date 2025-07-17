using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    // Разбегание
    public class BehaviourModeScatter : GhostBehaviorMode
    {
        public BehaviourModeScatter(MapHandlerService mapHandlerService)
            : base(mapHandlerService, GhostBehaviorModeType.Scatter) { }

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
            Dictionary<float, Vector2> directionsMap = CalculateDirectionsInScatterMode(availableDirections);
            Vector2 direction = SelectRandomDirection(directionsMap);
            return direction;
        }

        private Dictionary<float, Vector2> CalculateDirectionsInScatterMode(List<Vector2> availableDirections)
        {
            float maxDistance = float.MinValue;
            Dictionary<float, Vector2> directionsMap = new();

            foreach (var direction in availableDirections)
            {
                if (direction == -_selfDirection)
                    continue;

                float distance = _targetPosition.SqrDistance(_selfPosition + direction);
                directionsMap[distance] = direction;

                if (distance > maxDistance)
                    maxDistance = distance;
            }

            directionsMap.Remove(directionsMap.First(value => value.Key == maxDistance).Key);
            return directionsMap;
        }

        private Vector2 GetScatterPosition(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Blinky:
                    return Vector2.zero;                                    // Magic

                case EntityType.Pinky:
                    return new Vector2(29f, 0f);                            // Magic

                case EntityType.Inky:
                    return new Vector2(0f, -33f);                           // Magic

                case EntityType.Clyde:
                    return new Vector2(29f, -33f);                          // Magic

                default:
                    throw new System.Exception($"There is no implementation for this type: {entityType}");      // Magic
            }
        }
    }
}
