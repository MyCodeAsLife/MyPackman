using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    public abstract class GhostBehaviorMode
    {
        protected readonly MapHandlerService _mapHandlerService;

        protected Vector2 _selfPosition;
        protected Vector2 _enemyPosition;
        protected Vector2 _selfDirection;

        public GhostBehaviorMode(MapHandlerService mapHandlerService, GhostBehaviorModeType behaviorModeType)
        {
            _mapHandlerService = mapHandlerService;
            BehaviorType = behaviorModeType;
        }

        public GhostBehaviorModeType BehaviorType { get; private set; }

        public Vector2 CalculateDirectionOfMovement(Vector2 selfPosition, Vector2 selfDirection, Vector2 enemyPosition)
        {
            if (_mapHandlerService.IsCenterTail(selfPosition) == false)
                return selfDirection;

            _selfPosition = selfPosition;
            _enemyPosition = enemyPosition;
            _selfDirection = selfDirection;

            return CalculateDirection();
        }

        protected abstract Vector2 CalculateDirection();

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
