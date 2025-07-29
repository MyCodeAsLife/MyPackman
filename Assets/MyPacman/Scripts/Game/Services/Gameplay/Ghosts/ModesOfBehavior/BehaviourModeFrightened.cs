using R3;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Режим страха
    public class BehaviourModeFrightened : GhostBehaviorMode
    {
        public BehaviourModeFrightened(MapHandlerService mapHandlerService, Ghost self, ReadOnlyReactiveProperty<Vector2> pacmanPosition)
            : base(mapHandlerService, self, GhostBehaviorModeType.Frightened)
        {
            pacmanPosition.Subscribe(newPos => _targetPosition.OnNext(newPos)); // Будет ли ошибка если этот класс удалится а данная лямбда останется подписанна?
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)  // Похожа на такуюже в BehaviourModeScatter
        {
            Dictionary<float, Vector2> directionsMap = CalculateDirectionsClosestToTarget(availableDirections, _targetPosition.Value);
            directionsMap = RemoveWrongDirection(directionsMap, ItNear);
            return SelectRandomDirection(directionsMap);
        }
    }
}