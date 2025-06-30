using System;

namespace MyPacman
{
    [Serializable]
    public class EdibleData : EntityData
    {
        public EdibleEntityPoints Points { get; set; }
    }
}
