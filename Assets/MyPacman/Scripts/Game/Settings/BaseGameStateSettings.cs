using System.Collections.Generic;

namespace MyPacman
{
    public class BaseGameStateSettings
    {
        public GameStateData GetBaseGameState(ILevelConfig levelConfig)
        {
            var gameStateData = new GameStateData();
            gameStateData.Score = 0;
            gameStateData.LifePoints = GameConstants.StartLifePointsAmount;
            gameStateData.PickedFruits = new List<EntityType>();

            gameStateData.Map = new MapData();
            gameStateData.Map.LevelNumber = GameConstants.StartingDifficultyLevel;
            gameStateData.Map.NumberOfPellets = 0;
            gameStateData.Map.NumberOfCollectedPellets = 0;
            gameStateData.Map.MapTag = levelConfig.MapTag;

            return gameStateData;
        }
    }
}
