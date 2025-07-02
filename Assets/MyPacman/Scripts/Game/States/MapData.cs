using System;
using System.Collections.Generic;

namespace MyPacman
{
    [Serializable]
    public class MapData
    {
        public List<EntityData> Entities { get; set; } = new();
        public int LevelNumber { get; set; }                // Уровень сложности
        //public int NumberOfCollectedFruits {  get; set; }            // Кол-во фруктов на уровень
        public int NumberOfPellets {  get; set; }           // Кол-во гранул на уровень
        public int NumberOfCollectedPellets { get; set; }   // Кол-во подобранных за уровень гранул
    }
}