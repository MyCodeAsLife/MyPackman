using Game.State.Entities;
using System.Collections.Generic;

namespace Game.State.Maps
{
    public class MapData
    {
        public int Id { get; set; }
        public List<EntityData> Entities { get; set; }
    }
}
