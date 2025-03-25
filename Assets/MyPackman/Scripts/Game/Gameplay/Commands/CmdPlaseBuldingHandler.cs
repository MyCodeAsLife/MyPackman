using Game.State.Entities.Buildings;
using Game.State.cmd;
using Game.State.Root;
using Game.State.Buildings;

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

            var entityId = _gameState.GetEntityId();
            var newBuildingEntity = new BuildingEntity()
            {
                Id = entityId,
                Position = command.Position,
                TypeId = command.BuildingTypeId,
            };

            var newBuildingEntityProxy = new BuildingEntityProxy(newBuildingEntity);
            _gameState.Buildings.Add(newBuildingEntityProxy);

            return true;
        }
    }
}
