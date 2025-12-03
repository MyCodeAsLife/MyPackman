using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Режим возврата в загон
    public class BehaviourModeHomecomming : GhostBehaviorMode       // Вроде как ненужен этот класс
    {
        public BehaviourModeHomecomming(
            HandlerOfPickedEntities mapHandlerService,
            Ghost self,
            Vector2 targetPosition
            ) : base(mapHandlerService, self, GhostBehaviorModeType.Homecomming)
        {
            _targetPosition.OnNext(targetPosition);
        }

        protected override Vector2 CalculateDirection(List<Vector2> availableDirections = null)
        {
            availableDirections = _mapHandlerService.GetDirectionsWithoutWalls(_self.Position.Value);
            return base.CalculateDirection(availableDirections);
        }
    }
}
