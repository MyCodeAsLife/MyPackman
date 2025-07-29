using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    // Разбегание
    public class BehaviourModeScatter : GhostBehaviorMode
    {
        private List<Vector2> _gatePositions;

        private Func<List<Vector2>, Dictionary<float, Vector2>> CalculateDirections;

        public BehaviourModeScatter(
            MapHandlerService mapHandlerService,
            Ghost self,
            Vector2 targetPosition,
            GhostBehaviorModeType behaviorModeType)
            : base(mapHandlerService, self, behaviorModeType)
        {
            _targetPosition.OnNext(targetPosition);
            _gatePositions = mapHandlerService.GetTilePositions(GameConstants.GateTile);  // Таким же макаром получать позицию своего спавна?
            CalculateDirections = CalculateDirectionsToGatePosition;
        }

        protected override Vector2 CalculateDirection(List<Vector2> availableDirections = null)     // Похожа на себя в классе GhostBehaviorMode
        {
            if (availableDirections == null)
                availableDirections = _mapHandlerService.GetDirectionsWithoutWalls(_selfPosition);

            //if (availableDirections.Count == 1)
            //    return -_selfDirection;
            //else if (availableDirections.Count == 2)
            //    return availableDirections.First(value => value != -_selfDirection);
            //else if (_mapHandlerService.CheckTileForObstacle(_selfPosition))
            //    return _selfDirection;

            //return CalculateDirectionInSelectedMode(availableDirections);
            return base.CalculateDirection(availableDirections);
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)  // Похожа на такуюже в BehaviourModeFrightened
        {
            Dictionary<float, Vector2> directionsMap = CalculateDirections(availableDirections);
            return SelectRandomDirection(directionsMap);
        }

        private Dictionary<float, Vector2> CalculateDirectionsToScatterPosition(List<Vector2> availableDirections)
        {
            var calculateDirections = CalculateDirectionsClosestToTarget(availableDirections, _targetPosition.Value);
            return RemoveWrongDirection(calculateDirections, ItFar);
        }

        private Dictionary<float, Vector2> CalculateDirectionsToGatePosition(List<Vector2> availableDirections)
        {
            CheckNeighboringTilesForGates();
            Vector2 targetPosition = CalculatePositionNearestGate();
            var calculateDirections = CalculateDirectionsClosestToTarget(availableDirections, targetPosition);
            return RemoveWrongDirection(calculateDirections, ItFar);
        }

        private void CheckNeighboringTilesForGates()
        {
            var lastPosition = _selfPosition + -_selfDirection;

            if (_mapHandlerService.CheckTile(lastPosition, GameConstants.GateTile))
                CalculateDirections = CalculateDirectionsToScatterPosition;
        }

        private Vector2 CalculatePositionNearestGate()
        {
            Vector2 targetPosition = _gatePositions.First();
            float minDistance = targetPosition.SqrDistance(_selfPosition);

            foreach (var gatePosition in _gatePositions)
            {
                float distance = gatePosition.SqrDistance(_selfPosition);

                if (distance < minDistance)
                    targetPosition = gatePosition;
            }

            return targetPosition;
        }
    }
}