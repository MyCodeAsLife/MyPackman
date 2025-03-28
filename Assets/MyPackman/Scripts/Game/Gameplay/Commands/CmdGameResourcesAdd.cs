using Game.State.cmd;
using Game.State.GameResources;

namespace Game.Gameplay.Commands
{
    public class CmdGameResourcesAdd : ICommand
    {
        public readonly GameResourceType GameResourceType;
        public readonly int Amount;

        public CmdGameResourcesAdd(GameResourceType gameResourceType, int amount)
        {
            GameResourceType = gameResourceType;
            Amount = amount;
        }
    }
}
