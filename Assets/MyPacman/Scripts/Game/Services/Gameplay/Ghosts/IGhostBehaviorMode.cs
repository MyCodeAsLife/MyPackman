using UnityEngine;

namespace MyPacman
{
    public interface IGhostBehaviorMode
    {
        public GhostBehaviorModeType BehaviorType { get; }

        public Vector2 CalculateDirectionOfMovement(Vector2 selfPosition, Vector2 selfDirection, Vector2 enemyPosition);
    }
}
