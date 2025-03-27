using UnityEngine;

namespace Game.Settings.Gameplay.Maps
{
    [CreateAssetMenu(fileName = "MapSettings", menuName = "Game Settings/new Map Settings")]
    public class MapSettings : ScriptableObject
    {
        public int MapId;
        public MapInitialStateSettings InitialStateSettings;
    }
}
