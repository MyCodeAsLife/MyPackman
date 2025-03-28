using Game.State.cmd;
using Game.State.GameResources;

namespace Game.Gameplay.Commands
{
    public class CmdGameResourcesSpend : ICommand
    {
        public readonly GameResourceType GameResourceType;
        public readonly int Amount;

        public CmdGameResourcesSpend(GameResourceType gameResourceType, int amount)
        {
            GameResourceType = gameResourceType;
            Amount = amount;
        }
    }
}
