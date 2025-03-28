using Game.State.cmd;
using Game.State.GameResources;
using Game.State.Root;
using System.Linq;

namespace Game.Gameplay.Commands.Handlers
{
    public class CmdResourcesAddHandler : ICommandHandler<CmdGameResourcesAdd>
    {
        public readonly GameStateProxy _gameState;

        public CmdResourcesAddHandler(GameStateProxy gameState)
        {
            _gameState = gameState;
        }

        public bool Handle(CmdGameResourcesAdd command)
        {
            var requiredResourceType = command.GameResourceType;
            var requiredResource = _gameState.Resources.FirstOrDefault(r => r.GameResourceType == requiredResourceType);

            if (requiredResource == null)
            {
                requiredResource = CreateNewResource(requiredResourceType);
            }

            requiredResource.Amount.Value += command.Amount;
            return true;
        }

        private GameResource CreateNewResource(GameResourceType resourceType)
        {
            var newResorceData = new GameResourceData()
            {
                GameResourceType = resourceType,
                Amount = 0,
            };

            var newResource = new GameResource(newResorceData);
            _gameState.Resources.Add(newResource);

            return newResource;
        }
    }
}
