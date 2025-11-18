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
            self.Direction.Value = -self.Direction.Value;            // Резко разворачиваем призрака(или нужно было обнулить направление движения?)
            pacmanPosition.Subscribe(newPos => _targetPosition.OnNext(newPos)); // Будет ли ошибка если этот класс удалится а данная лямбда останется подписанна?
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections = null)  // Похожа на такуюже в BehaviourModeScatter
        {
            Debug.Log($"BehaviorMode - {Type}. Ghost - {_self.Type}");              //+++++++++++++++++++++++++++++
            // New
            availableDirections = _mapHandlerService.GetDirectionsWithoutObstacles(_self.Position.Value);

            Dictionary<float, Vector2> directionsMap = CalculateDirectionsClosestToTarget(availableDirections, _targetPosition.Value);
            directionsMap = RemoveWrongDirection(directionsMap, ItNear);
            return SelectRandomDirection(directionsMap);
        }

        //protected override Vector2 CalculateDirectionInSelectedMode()
        //{
        //    _targetPosition.OnNext(GetTarget());
        //    return base.CalculateDirectionInSelectedMode();
        //}

        //protected Vector2 GetTarget()
        //{
        //    return _pacmanPosition.CurrentValue;
        //}
    }
}