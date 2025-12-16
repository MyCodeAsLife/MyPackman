using R3;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Режим страха (работает)
    public class BehaviourModeFrightened : GhostBehaviorMode
    {
        private readonly ReadOnlyReactiveProperty<Vector2> _pacmanPosition;

        private Func<List<Vector2>, Vector2> DirectionCaliculation;

        public BehaviourModeFrightened(PickableEntityHandler mapHandlerService, Ghost self, ReadOnlyReactiveProperty<Vector2> pacmanPosition)
            : base(mapHandlerService, self, GhostBehaviorModeType.Frightened)
        {
            _pacmanPosition = pacmanPosition;
            DirectionCaliculation = FirstDirectionCalculation;         // Подменить DirectionCaliculation при первом прогоне, чтобы можно было сменить направление на противоположное от игрока
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)
        {
            _targetPosition.OnNext(_pacmanPosition.CurrentValue);
            return DirectionCaliculation(availableDirections);
        }

        private Vector2 FirstDirectionCalculation(List<Vector2> availableDirections)
        {
            DirectionCaliculation = SubsequentDirectionCalculations;
            Dictionary<float, Vector2> directionsMap = CalculateDirectionsMap(availableDirections, _targetPosition.Value);
            directionsMap = RemoveWrongDirection(directionsMap, ItNear);
            return SelectRandomDirection(directionsMap);
        }

        private Vector2 SubsequentDirectionCalculations(List<Vector2> availableDirections)
        {
            availableDirections = RemoveReverseDirection(availableDirections);
            var directionsMap = CalculateDirectionsMap(availableDirections, _targetPosition.Value);
            directionsMap = RemoveWrongDirection(directionsMap, ItNear);
            return SelectRandomDirection(directionsMap);
        }
    }
}