using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Режим разбегания
    public class BehaviourModeScatter : GhostBehaviorMode
    {
        private readonly Vector2 _scatterPos;
        private readonly Vector2 _blinkySpawnPos;

        private Func<List<Vector2>, Dictionary<float, Vector2>> CurrentAlgorithmForCalculatingDirections;
        private Func<Vector2, List<Vector2>> GetAvailableDirections;

        public BehaviourModeScatter(
            MapHandlerService mapHandlerService,
            Ghost self,
            Vector2 scatterPosition,
            GhostBehaviorModeType behaviorModeType,
            Vector2 blinkySpawnPos
            ) : base(mapHandlerService, self, behaviorModeType)
        {
            _scatterPos = scatterPosition;
            _blinkySpawnPos = blinkySpawnPos;
            BehaviourInitialize();
        }

        protected override Vector2 CalculateDirection(List<Vector2> availableDirections = null)
        {
            availableDirections = GetAvailableDirections(_self.Position.Value);
            return base.CalculateDirection(availableDirections);
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)  // Похожа на такуюже в BehaviourModeFrightened
        {
            Dictionary<float, Vector2> directionsMap = CurrentAlgorithmForCalculatingDirections(availableDirections);
            return SelectNearestDirection(directionsMap);
        }

        private void BehaviourInitialize()
        {
            if (_self.Position.Value == _blinkySpawnPos)
            {
                ChangeAlgorithm(
                    _scatterPos,
                    _mapHandlerService.GetDirectionsWithoutObstacles,
                    CalculateDirectionsToTargetPos);
            }
            else
            {
                ChangeAlgorithm(
                    _blinkySpawnPos,
                    _mapHandlerService.GetDirectionsWithoutWalls,
                    CalculateDirectionsToBlinkySpawnPos);
            }
        }

        private void ChangeAlgorithm(
            Vector2 targetPoint,
            Func<Vector2, List<Vector2>> GetAvailableDirectionsAlg,
            Func<List<Vector2>, Dictionary<float, Vector2>> CalculateDirectionsToTargetAlg)
        {
            _targetPosition.OnNext(targetPoint);
            GetAvailableDirections = GetAvailableDirectionsAlg;
            CurrentAlgorithmForCalculatingDirections = CalculateDirectionsToTargetAlg;
        }

        private Dictionary<float, Vector2> CalculateDirectionsToTargetPos(List<Vector2> availableDirections)
        {
            var calculateDirections = CalculateDirectionsClosestToTarget(availableDirections, _targetPosition.Value);
            return RemoveWrongDirection(calculateDirections, ItFar);
        }

        private Dictionary<float, Vector2> CalculateDirectionsToBlinkySpawnPos(List<Vector2> availableDirections)
        {
            if (_self.Position.Value.IsEnoughClose(_blinkySpawnPos, 0.5f)) // Magic    Данный метод срабатывает только когд движение достигает центра клетки, а позиция спавна находится на ее краю
                ChangeAlgorithm(
                    _scatterPos,
                    _mapHandlerService.GetDirectionsWithoutObstacles,
                    CalculateDirectionsToTargetPos);

            return CalculateDirectionsToTargetPos(availableDirections);
        }
    }
}