using System;
using UnityEngine;

namespace MyPacman
{
    [Serializable]
    public class PacmanData : EntityData, IMovableData
    {
        public PacmanData()
        {
            IsMoving = false;
        }

        public bool IsMoving { get; set; }
        public Vector2Int Direction { get; set; }
    }
}
