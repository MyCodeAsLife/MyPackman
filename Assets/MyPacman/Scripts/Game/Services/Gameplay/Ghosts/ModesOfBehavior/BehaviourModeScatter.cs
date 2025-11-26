using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Режим разбегания (работает)
    public class BehaviourModeScatter : GhostBehaviorMode
    {
        private readonly Vector2 _scatterPos;
        private readonly Vector2 _blinkySpawnPos;
        private readonly Vector2 _paddockCenter;

        private Func<List<Vector2>, Dictionary<float, Vector2>> CurrentAlgorithmForCalculatingDirections;
        private Func<Vector2, List<Vector2>> GetAvailableDirections;

        public BehaviourModeScatter(
            MapHandlerService mapHandlerService,
            Ghost self,
            Vector2 scatterPos,
            Vector2 paddockCenter,
            Vector2 blinkySpawnPos,
            GhostBehaviorModeType behaviorModeType
            ) : base(mapHandlerService, self, behaviorModeType)
        {
            _scatterPos = scatterPos;
            _paddockCenter = paddockCenter;
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
            if (_self.Position.Value.IsEnoughClose(_paddockCenter, 2.5f))   // Magic    Если расстояние до центра загона больше 2.5, то мы вне загона
            {
                ChangeAlgorithm(
                    _blinkySpawnPos,
                    _mapHandlerService.GetDirectionsWithoutWalls,
                    CalculateDirectionsToBlinkySpawnPos);
            }
            else
            {
                ChangeAlgorithm(
                    _scatterPos,
                    _mapHandlerService.GetDirectionsWithoutObstacles,
                    CalculateDirectionsToTargetPos);

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
            availableDirections = RemoveReverseDirection(availableDirections);
            var calculateDirections = CalculateDirectionsMap(availableDirections, _targetPosition.Value);
            return RemoveWrongDirection(calculateDirections, ItFar);
        }

        private Dictionary<float, Vector2> CalculateDirectionsToBlinkySpawnPos(List<Vector2> availableDirections)
        {
            // Заменить IsEnoughClose на sqrDistance ???
            if (_self.Position.Value.IsEnoughClose(_paddockCenter, 2.5f) == false)  // Magic    Если расстояние до центра загона больше 2.5, то мы вне загона
                ChangeAlgorithm(
                    _scatterPos,
                    _mapHandlerService.GetDirectionsWithoutObstacles,
                    CalculateDirectionsToTargetPos);

            return CalculateDirectionsToTargetPos(availableDirections);
        }
    }
}