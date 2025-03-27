namespace Game.Gameplay
{
    public class GameplayEnterParams : SceneEnterParams
    {
        public int MapId { get; }

        public GameplayEnterParams(int mapId) : base(ConstantsSceneNames.Gameplay)
        {
            MapId = mapId;
        }
    }
}