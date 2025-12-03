using R3;
using UnityEngine;

namespace MyPacman
{
    public class ClydeBehaviourModeChase : BehaviourModeChase
    {
        private readonly Vector2 _scatterPosition;

        public ClydeBehaviourModeChase(
            PickableEntityHandler mapHandlerService,
            Ghost self,
            ReadOnlyReactiveProperty<Vector2> pacmanPosition,
            Vector2 scatterPosition)
            : base(mapHandlerService, self, pacmanPosition)
        {
            _scatterPosition = scatterPosition;
        }

        protected override Vector2 GetTarget()
        {
            Vector2 target = _pacmanPosition.CurrentValue;

            if (_self.Position.Value.SqrDistance(target) < 8f)                                  // Magic
                return target;
            else
                return _scatterPosition;
        }
    }
}