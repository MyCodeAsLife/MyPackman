using System;
using UnityEngine;

namespace MyPacman
{
    [Serializable]
    public class PacmanEntityData : EntityData
    {
        public Vector2 AxisPosition {  get; set; }
    }
}
