using System;
using UnityEngine;

namespace MyPacman
{
    [Serializable]
    public class PacmanEntityData : EntityData
    {
        //public float PositionX { get; set; }
        //public float PositionY { get; set; }

        public Vector2Int Direction { get; set; }
    }
}
