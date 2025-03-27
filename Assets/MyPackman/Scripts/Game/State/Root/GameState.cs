using Game.State.Maps;
using System;
using System.Collections.Generic;

namespace Game.State.Root
{
    [Serializable]  // ��� ���������� ��������� ����, � ������������ � json
    public class GameState
    {
        public int GlobalEntityId;
        public int CurrentMapId;        // ����� ����� ���� ��������� ������� �����
        public List<MapState> Maps = new();

        public int CreateEntityId() => GlobalEntityId++;
    }
}