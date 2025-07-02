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
        public int NumberOfPellets {  get; set; }           // Кол-во гранул на уровень
        public int NumberOfCollectedPellets { get; set; }   // Кол-во подобранных за уровень гранул
    }
}