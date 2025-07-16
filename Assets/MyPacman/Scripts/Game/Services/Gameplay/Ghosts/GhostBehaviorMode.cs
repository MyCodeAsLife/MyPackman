using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    public abstract class GhostBehaviorMode
    {
        protected readonly MapHandlerService _mapHandlerService;

        protected Vector2 _selfPosition;
        protected Vector2 _targetPosition;
        protected Vector2 _selfDirection;

        public GhostBehaviorMode(MapHandlerService mapHandlerService, GhostBehaviorModeType behaviorModeType)
        {
            _mapHandlerService = mapHandlerService;
            Type = behaviorModeType;
        }

        public GhostBehaviorModeType Type { get; private set; }

        public Vector2 CalculateDirectionOfMovement(Vector2 selfPosition, Vector2 selfDirection, Vector2 enemyPosition)
        {
            if (_mapHandlerService.IsCenterTail(selfPosition) == false)
                return selfDirection;

            _selfPosition = selfPosition;
            _targetPosition = enemyPosition;
            _selfDirection = selfDirection;

            return CalculateDirection();
        }

        protected abstract Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections);

        protected virtual Vector2 CalculateDirection()
        {
            var availableDirections = _mapHandlerService.GetDirectionsWithoutObstacles(_selfPosition);

            if (availableDirections.Count == 1)
                return -_selfDirection;
            else if (availableDirections.Count == 2)
                return availableDirections.First(value => value != -_selfDirection);

            return CalculateDirectionInSelectedMode(availableDirections);
        }

        protected Vector2 SelectRandomDirection(Dictionary<float, Vector2> directionsMap)
        {
            Vector2 direction = Vector2.zero;
            int rand = Random.Range(0, directionsMap.Count);
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
    }
}
