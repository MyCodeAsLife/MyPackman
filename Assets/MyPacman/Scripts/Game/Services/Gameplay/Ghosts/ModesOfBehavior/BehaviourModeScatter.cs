using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Режим разбегания
    public class BehaviourModeScatter : GhostBehaviorMode
    {
        private bool _isCorral;

        private Func<List<Vector2>, Dictionary<float, Vector2>> CalculateDirections;

        public BehaviourModeScatter(
            MapHandlerService mapHandlerService,
            Ghost self,
            Vector2 targetPosition,
            GhostBehaviorModeType behaviorModeType,
            bool isCorral)
            : base(mapHandlerService, self, behaviorModeType)
        {
            _targetPosition.OnNext(targetPosition);
            _isCorral = isCorral;
            BehaviorInitialization();
        }

        protected override Vector2 CalculateDirection(List<Vector2> availableDirections = null)     // Похожа на себя в классе GhostBehaviorMode
        {
            if (_isCorral)
                availableDirections = _mapHandlerService.GetDirectionsWithoutWalls(_selfPosition);

            return base.CalculateDirection(availableDirections);
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)  // Похожа на такуюже в BehaviourModeFrightened
        {
            Dictionary<float, Vector2> directionsMap = CalculateDirections(availableDirections);
            return SelectRandomDirection(directionsMap);
        }

        private void BehaviorInitialization()
        {
            if (_isCorral)
            {
                var behavior = new BehaviorModePassageThroughGate(_mapHandlerService, _self, Type);
                behavior.GateReached += ChangeTargetBehavior;
                CalculateDirections = behavior.CalculateDirectionsToGatePosition;
            }
            else
            {
                CalculateDirections = CalculateDirectionsToScatterPosition;
            }
        }

        private void ChangeTargetBehavior()
        {
            _isCorral = false;
            CalculateDirections = CalculateDirectionsToScatterPosition;
        }

        private Dictionary<float, Vector2> CalculateDirectionsToScatterPosition(List<Vector2> availableDirections)
        {
            var calculateDirections = CalculateDirectionsClosestToTarget(availableDirections, _targetPosition.Value);
            return RemoveWrongDirection(calculateDirections, ItFar);
        }
    }
}