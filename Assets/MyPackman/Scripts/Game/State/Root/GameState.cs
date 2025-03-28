using Game.State.GameResources;
using Game.State.Maps;
using System;
using System.Collections.Generic;

namespace Game.State.Root
{
    [Serializable]  // Для сохранения состояния игры, и упаковывания в json
    public class GameState
    {
        private int _globalEntityId;

        public int CurrentMapId;        // Чтобы можно было сохранять текущую карту
        public List<MapState> Maps;
        public List<GameResourceData> GameResources;

        public int CreateEntityId() => _globalEntityId++;
    }
}