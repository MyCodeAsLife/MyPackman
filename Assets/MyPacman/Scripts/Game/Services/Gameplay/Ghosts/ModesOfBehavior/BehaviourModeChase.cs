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

        public BehaviourModeChase(MapHandlerService mapHandlerService, GhostBehaviorModeType behaviorModeType)
            : base(mapHandlerService, behaviorModeType) { }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)
        {
            throw new NotImplementedException();
        }
    }
}
