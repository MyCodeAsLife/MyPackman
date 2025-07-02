using System;
using UnityEngine;

namespace MyPacman
{
    [Serializable]
    public class PacmanData : EntityData
    {
        public float StartPositionX { get; set; }
        public float StartPositionY { get; set; }

        public Vector2Int Direction { get; set; }
    }
}
