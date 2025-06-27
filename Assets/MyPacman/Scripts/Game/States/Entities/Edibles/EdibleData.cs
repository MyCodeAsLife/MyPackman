using System;

namespace MyPacman
{
    [Serializable]
    public class EdibleData : EntityData
    {
        public EntityPoints Points { get; set; }
    }
}
