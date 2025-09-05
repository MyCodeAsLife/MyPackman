using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Режим возврата в загон
    public class BehaviourModeHomecomming : GhostBehaviorMode       // Вроде как ненужен этот класс
    {
        public BehaviourModeHomecomming(MapHandlerService mapHandlerService, Ghost self, Vector2 targetPosition)
            : base(mapHandlerService, self, GhostBehaviorModeType.Homecomming)
        {
            _targetPosition.OnNext(targetPosition);
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)
        {
            throw new NotImplementedException();
        }
    }
}
