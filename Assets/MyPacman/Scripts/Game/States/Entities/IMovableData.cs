using UnityEngine;

namespace MyPacman
{
    public interface IMovableData
    {
        public bool IsMoving { get; set; }
        public Vector2Int Direction { get; set; }
    }
}
