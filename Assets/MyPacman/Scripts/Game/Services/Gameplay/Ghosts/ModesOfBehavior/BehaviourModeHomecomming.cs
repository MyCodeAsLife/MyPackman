using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    public class BehaviourModeHomecomming : GhostBehaviorMode
    {
        public BehaviourModeHomecomming(MapHandlerService mapHandlerService)
            : base(mapHandlerService, GhostBehaviorModeType.Homecomming)
        {
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)
        {
            throw new NotImplementedException();
        }
    }
}
