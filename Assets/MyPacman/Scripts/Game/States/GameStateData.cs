using System;

namespace MyPacman
{
    [Serializable]
    public class GameStateData
    {
        public int GlobalEntityId { get; set; }                  // Счетчик для ID создаваемых сущностей.
        public MapData Map { get; set; }
        public int Score { get; set; }
        public int LifePoints { get; set; }
        public int NumberOfCollectedFruits { get; set; }

        public int CreateEntityId()
        {
            return GlobalEntityId++;
        }
    }
}