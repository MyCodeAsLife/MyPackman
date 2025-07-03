using System;
using System.Collections.Generic;

namespace MyPacman
{
    [Serializable]
    public class MapData
    {
        public string MapTag { get; set; }                  // Тэг используемой карты
        public List<EntityData> Entities { get; set; } = new();
        public int LevelNumber { get; set; }                // Уровень сложности
        public int NumberOfPellets { get; set; }           // Кол-во гранул на уровень
        public int NumberOfCollectedPellets { get; set; }   // Кол-во подобранных за уровень гранул
        public float PacmanSpawnPosX { get; set; }
        public float PacmanSpawnPosY { get; set; }
        public float BlinkySpawnPosX { get; set; }
        public float BlinkySpawnPosY { get; set; }
        public float PinkySpawnPosX { get; set; }
        public float PinkySpawnPosY { get; set; }
        public float InkySpawnPosX { get; set; }
        public float InkySpawnPosY { get; set; }
        public float ClydeSpawnPosX { get; set; }
        public float ClydeSpawnPosY { get; set; }
    }
}