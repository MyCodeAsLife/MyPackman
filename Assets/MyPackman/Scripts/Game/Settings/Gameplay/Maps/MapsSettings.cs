using System.Collections.Generic;
using UnityEngine;

namespace Game.Settings.Gameplay.Maps
{
    [CreateAssetMenu(fileName = "MapsSettings", menuName = "Game Settings/new Maps Settings")]
    public class MapsSettings : ScriptableObject
    {
        public List<MapSettings> Maps;
    }
}
