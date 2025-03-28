using Game.State.cmd;
using Game.State.Root;
using System.Linq;
using UnityEngine;

namespace Game.Gameplay.Commands.Handlers
{
    public class CmdResourcesSpendHandler : ICommandHandler<CmdGameResourcesSpend>
    {
        public readonly GameStateProxy _gameState;

        public CmdResourcesSpendHandler(GameStateProxy gameState)
        {
            _gameState = gameState;
        }

        public bool Handle(CmdGameResourcesSpend command)
        {
            var requiredResourceType = command.GameResourceType;
            var requiredResource = _gameState.Resources.FirstOrDefault(r => r.GameResourceType == requiredResourceType);

            if (requiredResource == null)
            {
                Debug.Log("Trying to spend not existed resource");  // For tests
                return false;
            }

            if (requiredResource.Amount.Value < command.Amount)
            {
                Debug.LogError($"Trying to spend more resources than existed ({requiredResourceType}). " +
                               $"Exist: {requiredResource.Amount.Value}");  // For tests
                return false;
            }

            requiredResource.Amount.Value -= command.Amount;
            return true;
        }
    }
}
