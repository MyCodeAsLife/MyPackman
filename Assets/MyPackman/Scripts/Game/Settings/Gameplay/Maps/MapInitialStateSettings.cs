using Game.Settings.Gameplay.Buildings;
using System;
using System.Collections.Generic;

namespace Game.Settings.Gameplay.Maps
{
    [Serializable]
    public class MapInitialStateSettings
    {
        public List<BuildingInitialStateSettings> InitialBuildings;
    }
}
