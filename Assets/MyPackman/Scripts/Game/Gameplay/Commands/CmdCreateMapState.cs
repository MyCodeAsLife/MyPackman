using Game.State.cmd;

namespace Game.Gameplay.Commands
{
    public class CmdCreateMapState : ICommand
    {
        public readonly int MapId;

        public CmdCreateMapState(int mapId)
        {
            MapId = mapId;
        }
    }
}
