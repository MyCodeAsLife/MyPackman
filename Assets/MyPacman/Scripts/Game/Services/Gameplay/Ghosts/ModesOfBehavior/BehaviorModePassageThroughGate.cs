using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    // Или сделать этот мод частью разбегания, но проверять начинает ли призрак в загоне или нет
    // И отталкиваясь от этого создавать этот класс
    public class BehaviorModePassageThroughGate : GhostBehaviorMode
    {
        private readonly IReadOnlyList<Vector2> _gatePositions;

        public event Action GateReached;

        public BehaviorModePassageThroughGate(
            MapHandlerService mapHandlerService,
            Ghost self,
            GhostBehaviorModeType behaviorModeType)
            : base(mapHandlerService, self, behaviorModeType)
        {
            _gatePositions = mapHandlerService.GatePositions;
        }

        protected override Vector2 CalculateDirection(List<Vector2> availableDirections = null)     // Похожа на себя в классе GhostBehaviorMode
        {
            if (availableDirections == null)
                availableDirections = _mapHandlerService.GetDirectionsWithoutWalls(_selfPosition);

            return base.CalculateDirection(availableDirections);
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)
        {
            Dictionary<float, Vector2> directionsMap = CalculateDirectionsToGatePosition(availableDirections);
            Vector2 availableDirection = directionsMap.First().Value;
            float minDistance = directionsMap.First().Key;

            foreach (var direction in directionsMap)
                if (direction.Key < minDistance)
                    availableDirection = direction.Value;

            //return SelectRandomDirection(directionsMap);
            return availableDirection;
        }

        public Dictionary<float, Vector2> CalculateDirectionsToGatePosition(List<Vector2> availableDirections)
        {
            CheckNeighboringTilesForGates();
            Vector2 targetPosition = CalculatePositionNearestGate();
            var calculateDirections = CalculateDirectionsClosestToTarget(availableDirections, targetPosition);
            return RemoveWrongDirection(calculateDirections, ItFar);
        }

        private void CheckNeighboringTilesForGates()
        {
            var lastPosition = _selfPosition + -_selfDirection;

            // Посылать команду что ворота достигнуты
            if (_mapHandlerService.CheckTile(lastPosition, GameConstants.GateTile))
                GateReached?.Invoke();
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
