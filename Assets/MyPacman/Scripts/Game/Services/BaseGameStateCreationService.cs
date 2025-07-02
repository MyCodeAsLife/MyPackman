namespace MyPacman
{
    public class BaseGameStateCreationService
    {
        public GameStateData Create(ILevelConfig levelConfig)
        {
            var gameStateData = new GameStateData();
            gameStateData.Score = 0;
            gameStateData.HigthScore = 0;                                       // Подгружать из сохранения
            gameStateData.LifePoints = GameConstants.StartLifePointsAmount;
            gameStateData.NumberOfCollectedFruits = 0;

            gameStateData.Map = new MapData();
            gameStateData.Map.LevelNumber = GameConstants.StartingDifficultyLevel;
            gameStateData.Map.NumberOfPellets = 0;
            gameStateData.Map.NumberOfCollectedPellets = 0;
            gameStateData.Map.MapTag = levelConfig.MapTag;

            return gameStateData;
        }
    }
}
