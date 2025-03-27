using Game.State.Entities.Buildings;
using System;
using System.Collections.Generic;

namespace Game.State.Maps
{
    [Serializable]
    public class MapState
    {
        public int Id;
        public List<BuildingEntity> Buildings;
    }
}
