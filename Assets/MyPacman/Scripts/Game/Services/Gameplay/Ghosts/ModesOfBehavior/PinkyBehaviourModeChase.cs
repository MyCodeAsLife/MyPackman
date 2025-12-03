using R3;
using UnityEngine;

namespace MyPacman
{
    public class PinkyBehaviourModeChase : BehaviourModeChase
    {
        private readonly ReadOnlyReactiveProperty<Vector2> _pacmanDirection;

        public PinkyBehaviourModeChase(
            PickableEntityHandler mapHandlerService,
            Ghost self,
            ReadOnlyReactiveProperty<Vector2> pacmanPosition,
            ReadOnlyReactiveProperty<Vector2> pacmanDirection)
            : base(mapHandlerService, self, pacmanPosition)
        {
            _pacmanDirection = pacmanDirection;
        }

        protected override Vector2 GetTarget()
        {
            return _pacmanPosition.CurrentValue + _pacmanDirection.CurrentValue * 4;            // Magic
        }
    }
}
