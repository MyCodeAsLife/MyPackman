using System;

namespace MyPacman
{
    [Serializable]
    public class PacmanEntityData : EntityData
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
    }
}
