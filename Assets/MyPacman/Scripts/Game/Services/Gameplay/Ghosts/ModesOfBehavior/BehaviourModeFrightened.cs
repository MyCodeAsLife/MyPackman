using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    // Режим страха
    public class BehaviourModeFrightened : GhostBehaviorMode
    {
        private ReadOnlyReactiveProperty<Vector2> _pacmanPosition;     // Это нужно?

        public BehaviourModeFrightened(MapHandlerService mapHandlerService, Ghost self, ReadOnlyReactiveProperty<Vector2> pacmanPosition)
            : base(mapHandlerService, self, GhostBehaviorModeType.Frightened)
        {
            _pacmanPosition = pacmanPosition;
            pacmanPosition.Subscribe(newPos => _targetPosition.OnNext(newPos)); // Будет ли ошибка если этот класс удалится а данная лямбда останется подписанна?
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)  // Похожа на такуюже в BehaviourModeScatter
        {
            Dictionary<float, Vector2> directionsMap = CalculateDirectionsInFrightenedMode(availableDirections);
            Vector2 direction = SelectRandomDirection(directionsMap);
            return direction;
        }

        private Dictionary<float, Vector2> CalculateDirectionsInFrightenedMode(List<Vector2> availableDirections)
        {
            float minDistance = float.MaxValue;
            Dictionary<float, Vector2> directionsMap = new();

            foreach (var direction in availableDirections)
            {
                if (direction == -_selfDirection)
                    continue;

                float distance = _targetPosition.Value.SqrDistance(_selfPosition + direction);
                directionsMap[distance] = direction;

                if (distance < minDistance)
                    minDistance = distance;
            }

            directionsMap.Remove(directionsMap.First(value => value.Key == minDistance).Key);
            return directionsMap;
        }
    }
}