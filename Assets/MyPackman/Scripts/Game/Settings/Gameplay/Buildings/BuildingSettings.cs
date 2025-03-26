using System.Collections.Generic;
using UnityEngine;

namespace Game.Settings.Gameplay.Buildings
{
    [CreateAssetMenu(fileName = "BuildingSettings", menuName = "Game Settings/new Building Settings")]
    public class BuildingSettings : ScriptableObject
    {
        public string TypeId;
        public string TitleId;
        public string DescriptionId;
        public List<BuildingLevelSettings> LevelSettings;
    }
}
