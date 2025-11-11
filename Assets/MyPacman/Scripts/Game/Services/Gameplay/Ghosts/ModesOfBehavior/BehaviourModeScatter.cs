using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Режим разбегания
    public class BehaviourModeScatter : GhostBehaviorMode
    {
        private Func<List<Vector2>, Dictionary<float, Vector2>> CurrentAlgorithmForCalculatingDirections;

        public BehaviourModeScatter(
            MapHandlerService mapHandlerService,
            Ghost self,
            Vector2 targetPosition,
            GhostBehaviorModeType behaviorModeType
            ) : base(mapHandlerService, self, behaviorModeType)
        {
            _targetPosition.OnNext(targetPosition);
            BehaviorInitialization();
        }
        // Остановился тут
        // 1. Проверить Достигнута ли точка спавна красного
        // 1.1 Если да то выбрать алгоритм движения к целевой точке (проход через ворота невозможен)
        // 1.2 Если нет то выбрать алгоритм движения к точке спавна красного (приоритет на движение в сторону ворот)
        // 1.2.1 Каждый цикл начала движения проверять, достигну та ли точка спавна красного
        // 1.2.1.1 Если да то 1.1

        // Доделать
        protected override Vector2 CalculateDirection(List<Vector2> availableDirections = null)
        {
            availableDirections = _mapHandlerService.GetDirectionsWithoutWalls(_selfPosition);
            return base.CalculateDirection(availableDirections);
        }
        // Доделать
        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)  // Похожа на такуюже в BehaviourModeFrightened
        {
            Dictionary<float, Vector2> directionsMap = CurrentAlgorithmForCalculatingDirections(availableDirections);
            return SelectRandomDirection(directionsMap);
        }
        // Доделать
        private Dictionary<float, Vector2> CalculateDirectionsToScatterPosition(List<Vector2> availableDirections)
        {
            var calculateDirections = CalculateDirectionsClosestToTarget(availableDirections, _targetPosition.Value);
            return RemoveWrongDirection(calculateDirections, ItFar);
        }
        // Доделать
        private void BehaviorInitialization()
        {
            if (_isCorral)
                CurrentAlgorithmForCalculatingDirections = behavior.CalculateDirectionsToGatePosition;
            else
                CurrentAlgorithmForCalculatingDirections = CalculateDirectionsToScatterPosition;
        }

        //protected override Dictionary<float, Vector2> CalculateDirectionInSelectedMode(
        //    List<Vector2> availableDirections = null)
        //{
        //    availableDirections = _mapHandlerService.GetDirectionsWithoutObstacles(_selfPosition);
        //    var calculatedDirections = base.CalculateDirectionInSelectedMode(availableDirections);

        //    var directionsMap = RemoveWrongDirection(calculatedDirections, ItFar);
        //    return SelectRandomDirection(directionsMap);
        //}
    }
}