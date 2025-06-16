using System;
using System.Collections.Generic;

namespace MyPacman
{
    [Serializable]
    public class MapData
    {
        public int Id { get; set; }
        public List<EntityData> Entities { get; set; }
    }
}