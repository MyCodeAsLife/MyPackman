using System;

namespace MyPacman
{
    [Serializable]
    public class GameStateData
    {
        public int GlobalEntityId { get; set; }                  // Счетчик для ID создаваемых сущностей.
        public int CurrentMapId { get; set; }
        public MapData Map { get; set; }
        public ScoreData Score { get; set; }
        public ScoreData HigthScore { get; set; }
        public LifePointData LifePoints { get; set; }

        public int CreateEntityId()
        {
            return GlobalEntityId++;
        }
    }
}