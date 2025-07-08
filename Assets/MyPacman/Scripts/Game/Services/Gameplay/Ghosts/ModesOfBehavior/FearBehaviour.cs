using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    public class FearBehaviour : IGhostBehaviorMode
    {
        private readonly Ghost _self;
        private readonly Pacman _enemy;
        private readonly MapHandlerService _mapHandlerService;

        public FearBehaviour(Ghost ghost, Pacman pacman, MapHandlerService mapHandlerService)
        {
            _self = ghost;
            _enemy = pacman;
            _mapHandlerService = mapHandlerService;
        }

        public Vector2 CalculatePointOfMovement()
        {
            var points = _mapHandlerService.GetAvailableMovementPoints(_self.Position.Value);
            Dictionary<float, Vector2> pointsMap = new();
            float minDistance = float.MaxValue;

            foreach (var point in points)
            {
                var newDistance = point.SqrDistance(_enemy.Position.Value);
                pointsMap[newDistance] = point;

                if (newDistance < minDistance)
                    minDistance = newDistance;
            }

            if (pointsMap.Count > 2)
            {
                pointsMap.Remove(minDistance);
                var selectPoint = Random.Range(0, pointsMap.Count);
                int counter = 0;

                foreach (var point in pointsMap)
                {
                    if (counter == selectPoint)
                        return point.Value;

                    counter++;
                }
            }
            else
            {
                foreach (var point in pointsMap)
                {
                    if (point.Key != minDistance)
                        return point.Value;
                }
            }

            throw new System.Exception("Failed to find a point for movement");
        }
    }
}
