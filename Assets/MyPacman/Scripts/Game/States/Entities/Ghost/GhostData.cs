using UnityEngine;

namespace MyPacman
{
    public class GhostData : EdibleData, IMovableData
    {
        //public GhostData()
        //{
        //    IsMoving = false;
        //}

        public bool IsMoving { get; set; } = false;
        public Vector2Int Direction { get; set; } = Vector2Int.right;       // Start direction movement
        public float SpeedModifier { get; set; } = GameConstants.GhostStartingSpeed​​Modifier;
    }
}
