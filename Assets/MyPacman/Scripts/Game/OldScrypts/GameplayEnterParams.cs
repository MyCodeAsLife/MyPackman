namespace MyPacman
{
    public class GameplayEnterParams : SceneEnterParams
    {
        public GameplayEnterParams(string saveFileName, ILevelConfig levelConfig) : base(levelConfig)
        {
            SaveFileName = saveFileName;
            //MapId = mapId;
        }

        public string SaveFileName { get; }
        //public int MapId { get; }
    }
}