using R3;
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

        protected override Vector2 CalculateDirectionInSelectedMode()
        {
            _targetPosition.OnNext(GetTarget());
            return base.CalculateDirectionInSelectedMode();
        }

        protected abstract Vector2 GetTarget();
    }
}
