using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Преследование
    public class BehaviourModeChase : GhostBehaviorMode
    {
        // Смещение для расчета целевой точки, для каждого призрака свое.
        private Vector2 _targetPointOffset;
        private Entity _pacman;

        public BehaviourModeChase(MapHandlerService mapHandlerService, Ghost self, Entity pacman)
            : base(mapHandlerService, self, GhostBehaviorModeType.Chase)
        {
            _pacman = pacman;
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)
        {
            throw new NotImplementedException();
        }
    }
}
