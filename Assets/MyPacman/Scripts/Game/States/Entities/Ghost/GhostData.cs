using UnityEngine;

namespace MyPacman
{
    public class GhostData : EdibleData, IMovableData
    {
        public bool IsMoving { get; set; } = false;
        public Vector2Int Direction { get; set; } = Vector2Int.right;       // Start direction movement
        public float SpeedModifier { get; set; } = GameConstants.GhostNormalSpeed​​Modifier;
        // New
        public GhostBehaviorModeType CurrentBehaviorMode { get; set; }          // Переименовать
        public float BehaviorTimer { get; set; }              // Таймер до завершения текущего поведения (пока не используется)
    }
}
