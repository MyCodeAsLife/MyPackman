using R3;
using UnityEngine;

namespace MyPacman
{
    public class InkyBehaviourModeChase : BehaviourModeChase
    {
        private readonly ReadOnlyReactiveProperty<Vector2> _blinkyPosition;

        public InkyBehaviourModeChase(
            PickableEntityHandler mapHandlerService,
            Ghost self,
            ReadOnlyReactiveProperty<Vector2> pacmanPosition,
            ReadOnlyReactiveProperty<Vector2> blinkyPosition)
            : base(mapHandlerService, self, pacmanPosition)
        {
            _blinkyPosition = blinkyPosition;
        }

        protected override Vector2 GetTarget()
        {
            return _pacmanPosition.CurrentValue + (_pacmanPosition.CurrentValue - _blinkyPosition.CurrentValue);
        }
    }
}