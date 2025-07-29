using R3;
using UnityEngine;

namespace MyPacman
{
    public class BlinkyBehaviourModeChase : BehaviourModeChase
    {
        public BlinkyBehaviourModeChase(
            MapHandlerService mapHandlerService,
            Ghost self,
            ReadOnlyReactiveProperty<Vector2> pacmanPosition)
            : base(mapHandlerService, self, pacmanPosition)
        {
        }

        protected override Vector2 GetTarget()
        {
            return _pacmanPosition.CurrentValue;
        }
    }
}
