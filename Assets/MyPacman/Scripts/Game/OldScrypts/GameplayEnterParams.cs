namespace MyPacman
{
    public class GameplayEnterParams : SceneEnterParams
    {
        public GameplayEnterParams(string saveFileName, int mapId) : base(GameConstants.GameplayScene)
        {
            SaveFileName = saveFileName;
            MapId = mapId;
        }

        public string SaveFileName { get; }
        public int MapId { get; }
    }
}