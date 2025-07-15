using System;
using UnityEngine;

namespace MyPacman
{
    // Преследование
    public class BehaviourModeChase : GhostBehaviorMode
    {
        public BehaviourModeChase(MapHandlerService mapHandlerService, GhostBehaviorModeType behaviorModeType)
            : base(mapHandlerService, behaviorModeType) { }

        protected override Vector2 CalculateDirection()
        {
            throw new NotImplementedException();
        }
    }
}
