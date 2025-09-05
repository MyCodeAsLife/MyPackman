using R3;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Режим преследования
    public abstract class BehaviourModeChase : GhostBehaviorMode
    {
        protected readonly ReadOnlyReactiveProperty<Vector2> _pacmanPosition;

        public BehaviourModeChase(
            MapHandlerService mapHandlerService,
            Ghost self,
            ReadOnlyReactiveProperty<Vector2> pacmanPosition)
            : base(mapHandlerService, self, GhostBehaviorModeType.Chase)
        {
            _pacmanPosition = pacmanPosition;
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)
        {
            _targetPosition.OnNext(GetTarget());
            var calculateDirections = CalculateDirectionsClosestToTarget(availableDirections, _targetPosition.Value);
            //calculateDirections = RemoveWrongDirection(calculateDirections, ItFar);
            return SelectNearestDirection(calculateDirections);
        }

        protected abstract Vector2 GetTarget();

        private Vector2 SelectNearestDirection(Dictionary<float, Vector2> availableDirections)
        {
            float maxDistance = float.MaxValue;

            foreach (var direction in availableDirections)
                if (direction.Key < maxDistance)
                    maxDistance = direction.Key;

            return availableDirections[maxDistance];
        }
    }
}
