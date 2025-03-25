using Game.State.Entities.Buildings;
using System;
using System.Collections.Generic;

namespace Game.State.Root
{
    [Serializable]  // ��� ���������� ��������� ����, � ������������ � json
    public class GameState
    {
        public int GlobalEntityId;
        public List<BuildingEntity> Buildings = new();
    }
}