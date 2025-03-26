using UnityEngine;

namespace Game.Settings.Gameplay.Buildings
{
    [CreateAssetMenu(fileName = "BuildingLevelSettings", menuName = "Game Settings/new Building Level Settings")]
    public class BuildingLevelSettings : ScriptableObject
    {
        public int Level;
        public double BaseIncome;
    }
}
