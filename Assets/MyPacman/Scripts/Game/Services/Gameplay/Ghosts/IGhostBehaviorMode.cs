using UnityEngine;

namespace MyPacman
{
    public interface IGhostBehaviorMode
    {
        public Vector2 CalculatePointOfMovement();
    }
}
