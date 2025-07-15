using System;
using UnityEngine;

namespace MyPacman
{
    // Разбегание
    public class BehaviourModeScatter : GhostBehaviorMode
    {
        public BehaviourModeScatter(MapHandlerService mapHandlerService, GhostBehaviorModeType behaviorModeType)
            : base(mapHandlerService, behaviorModeType) { }

        // Проход сквозь барьер втиснуть в разбегание
        protected override Vector2 CalculateDirection()
        {
            throw new NotImplementedException();
        }
    }
}
