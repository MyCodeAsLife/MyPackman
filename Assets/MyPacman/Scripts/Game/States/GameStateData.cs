using System;
using System.Collections.Generic;

namespace MyPacman
{
    [Serializable]
    public class GameStateData
    {
        public int GlobalEntityId { get; set; }                  // Счетчик для ID создаваемых сущностей.
        public MapData Map { get; set; }
        public int Score { get; set; }
        public int LifePoints { get; set; }
        public List<EntityType> PickedFruits { get; set; } = new();  // Зачем "съеденные" фрукты хранить как список? Не проше ли просто посчитать как int?
        // New
        public float LevelTimeHasPassed { get; set; }

        public int CreateEntityId() => GlobalEntityId++;
    }
}