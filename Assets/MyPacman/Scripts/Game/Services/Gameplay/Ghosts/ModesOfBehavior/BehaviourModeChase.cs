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

        public BehaviourModeChase(MapHandlerService mapHandlerService)
            : base(mapHandlerService, GhostBehaviorModeType.Chase) { }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)
        {
            throw new NotImplementedException();
        }
    }
}
