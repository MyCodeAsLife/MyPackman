using R3;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Режим преследования (неправильно работает с Clyde смотреть в его реализации)
    public abstract class BehaviourModeChase : GhostBehaviorMode
    {
        protected readonly ReadOnlyReactiveProperty<Vector2> _pacmanPosition;

        public BehaviourModeChase(
            PickableEntityHandler mapHandlerService,
            Ghost self,
            ReadOnlyReactiveProperty<Vector2> pacmanPosition)
            : base(mapHandlerService, self, GhostBehaviorModeType.Chase)
        {
            _pacmanPosition = pacmanPosition;
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)
        {
            _targetPosition.OnNext(GetTarget());
            return base.CalculateDirectionInSelectedMode(availableDirections);
        }

        protected abstract Vector2 GetTarget();
    }
}
