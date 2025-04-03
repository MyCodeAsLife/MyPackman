using Game.State.cmd;

namespace Game.Gameplay.Commands
{
    public class CmdCreateMap : ICommand
    {
        public readonly int MapId;

        public CmdCreateMap(int mapId)
        {
            MapId = mapId;
        }
    }
}
