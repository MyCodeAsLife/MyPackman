using Game.State.Buildings;
using Game.State.cmd;
using Game.State.Entities.Buildings;
using Game.State.Root;
using System.Linq;
using UnityEngine;

namespace Game.Gameplay.Commands
{
    public class CmdPlaseBuldingHandler : ICommandHandler<CmdPlaceBuilding>
    {
        private readonly GameStateProxy _gameState;

        public CmdPlaseBuldingHandler(GameStateProxy gameState)
        {
            _gameState = gameState;
        }

        public bool Handle(CmdPlaceBuilding command)
        {
            // Тут можно сначало провести валидацию: достаточно ли ресурсов, свободно ли место и т.д.

            // Затем ищем текущую карту (передавать текущую карту в команде вместо того чтобы искать ее?)
            var currentMap = _gameState.Maps.FirstOrDefault(map => map.Id == _gameState.CurrentMapId.CurrentValue);

            if (currentMap == null)
            {
                Debug.Log($"Couldn't find MapState for id: {_gameState.CurrentMapId.CurrentValue}");
                return false;
            }

            var entityId = _gameState.CreateEntityId();
            var newBuildingEntity = new BuildingEntity()
            {
                Id = entityId,
                Position = command.Position,
                TypeId = command.BuildingTypeId,
            };

            var newBuildingEntityProxy = new BuildingEntityProxy(newBuildingEntity);
            currentMap.Buildings.Add(newBuildingEntityProxy);

            return true;
        }
    }
}
