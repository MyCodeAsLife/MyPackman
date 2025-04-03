﻿using System;
using UnityEngine;

namespace Game.Settings.Gameplay.Buildings
{
    [Serializable]
    public class BuildingInitialStateSettings   // Состояние здания по умолчанию
    {
        public string ConfigId;
        public int Level;
        public Vector2Int Position;
    }
}
