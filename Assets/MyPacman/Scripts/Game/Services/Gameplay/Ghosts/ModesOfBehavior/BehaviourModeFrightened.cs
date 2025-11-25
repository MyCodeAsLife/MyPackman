using R3;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Режим страха (работает)
    public class BehaviourModeFrightened : GhostBehaviorMode
    {
        private Func<List<Vector2>, Vector2> DirectionCaliculation;

        public BehaviourModeFrightened(MapHandlerService mapHandlerService, Ghost self, ReadOnlyReactiveProperty<Vector2> pacmanPosition)
            : base(mapHandlerService, self, GhostBehaviorModeType.Frightened)
        {
            pacmanPosition.Subscribe(newPos => _targetPosition.OnNext(newPos)); // Будет ли ошибка если этот класс удалится а данная лямбда останется подписанна?
            DirectionCaliculation = FirstDirectionCalculation;         // Подменить DirectionCaliculation при первом прогоне, чтобы можно было сменить направление на противоположное от игрока
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)
        {
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

        //protected Vector2 GetTarget()
        //{
        //    return _pacmanPosition.CurrentValue;
        //}
    }
}