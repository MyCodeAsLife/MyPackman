using Game.State.Buildings;
using System;
using System.Collections.Generic;

namespace Game.State.Root
{
    [Serializable]  // ��� ���������� ��������� ����, � ������������ � json
    public class GameState
    {
        public List<BuildingEntity> Buildings = new();
    }
}