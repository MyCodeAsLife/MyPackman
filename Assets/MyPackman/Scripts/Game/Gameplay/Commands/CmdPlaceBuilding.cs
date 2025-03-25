using Game.State.cmd;
using UnityEngine;

namespace Game.Gameplay.Commands
{
    // Комманда для постройки строения
    public class CmdPlaceBuilding : ICommand
    {
        public readonly string BuildingTypeId;  // Тип строения
        public readonly Vector3Int Position;    // Позиция где строить

        public CmdPlaceBuilding(string buildingTypeId, Vector3Int position)
        {
            BuildingTypeId = buildingTypeId;
            Position = position;
        }
    }
}