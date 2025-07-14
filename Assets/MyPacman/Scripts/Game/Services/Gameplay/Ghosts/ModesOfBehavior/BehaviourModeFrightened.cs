using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    public class BehaviourModeFrightened : IGhostBehaviorMode
    {
        private readonly MapHandlerService _mapHandlerService;

        private Vector2 _selfPosition;
        //private Vector2 _selfDirection;
        private Vector2 _enemyPosition;

        public BehaviourModeFrightened(MapHandlerService mapHandlerService)
        {
            _mapHandlerService = mapHandlerService;
        }

        public GhostBehaviorModeType BehaviorType { get; private set; } = GhostBehaviorModeType.Frightened;

        public Vector2 CalculateDirectionOfMovement(Vector2 selfPosition, Vector2 selfDirection, Vector2 enemyPosition)
        {
            _selfPosition = selfPosition;
            _enemyPosition = enemyPosition;
            //_selfDirection = selfDirection;

            if (_mapHandlerService.IsCenterTail(_selfPosition) == false)
                return selfDirection;

            return CalculateDirection();
        }

        private Vector2 CalculateDirection()
        {
            var availableDirections = _mapHandlerService.GetDirectionsWithoutObstacles(_selfPosition);

            if (availableDirections.Count == 1)
                return availableDirections[0];

            Dictionary<float, Vector2> directionsMap = CreateDirectionsMap(availableDirections);
            return SelectRandomDirection(directionsMap);
        }

        private Dictionary<float, Vector2> CreateDirectionsMap(List<Vector2> availableDirections)
        {
            float minDistance = float.MaxValue;
            Dictionary<float, Vector2> directionsMap = new();

            foreach (var direction in availableDirections)
            {
                float distance = _enemyPosition.SqrDistance(_selfPosition + direction);
                directionsMap[distance] = direction;

                if (distance < minDistance)
                    minDistance = distance;
            }

            directionsMap.Remove(directionsMap.First(value => value.Key == minDistance).Key);
            return directionsMap;
        }

        private Vector2 SelectRandomDirection(Dictionary<float, Vector2> directionsMap)
        {
            Vector2 direction = Vector2.zero;
            int rand = Random.Range(0, directionsMap.Count);
            int counter = 0;

            foreach (var selectDirection in directionsMap)
            {
                if (rand == counter)
                    direction = selectDirection.Value;

                counter++;
            }

            return direction;
        }
    }
}