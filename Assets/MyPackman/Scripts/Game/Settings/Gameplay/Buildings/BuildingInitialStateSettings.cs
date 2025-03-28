using System;
using UnityEngine;

namespace Game.Settings.Gameplay.Buildings
{
    [Serializable]
    public class BuildingInitialStateSettings   // Состояние здания по умолчанию
    {
        public string TypeId;
        public int Level;
        public Vector3Int Position;
    }
}
