using Game.State.cmd;
using UnityEngine;

namespace Game.Gameplay.Commands
{
    // �������� ��� ��������� ��������
    public class CmdPlaceBuilding : ICommand
    {
        public readonly string BuildingTypeId;  // ��� ��������
        public readonly Vector3Int Position;    // ������� ��� �������

        public CmdPlaceBuilding(string buildingTypeId, Vector3Int position)
        {
            BuildingTypeId = buildingTypeId;
            Position = position;
        }
    }
}