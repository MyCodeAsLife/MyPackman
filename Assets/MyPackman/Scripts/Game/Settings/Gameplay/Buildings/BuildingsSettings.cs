﻿using System.Collections.Generic;
using UnityEngine;

namespace Game.Settings.Gameplay.Buildings
{
    [CreateAssetMenu(fileName = "BuildingsSettings", menuName = "Game Settings/new Buildings Settings")]
    public class BuildingsSettings : ScriptableObject
    {
        public List<BuildingSettings> AllBuildings;
    }
}
