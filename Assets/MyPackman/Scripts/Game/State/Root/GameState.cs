using Game.State.GameResources;
using Game.State.Maps;
using System;
using System.Collections.Generic;

namespace Game.State.Root
{
    [Serializable]  // ��� ���������� ��������� ����, � ������������ � json
    public class GameState
    {
        private int _globalEntityId;

        public int CurrentMapId;        // ����� ����� ���� ��������� ������� �����
        public List<MapState> Maps;
        public List<GameResourceData> GameResources;

        public int CreateEntityId() => _globalEntityId++;
    }
}