namespace Game.Gameplay
{
    public class GameplayEnterParams : SceneEnterParams
    {
        public string SaveFileName { get; }
        public int LevelNumber { get; }

        public GameplayEnterParams(string saveFileName, int levelNumber) : base(ConstantsSceneNames.Gameplay)
        {
            SaveFileName = saveFileName;
            LevelNumber = levelNumber;
        }
    }
}