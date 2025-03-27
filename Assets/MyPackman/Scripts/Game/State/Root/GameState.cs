using Game.State.Maps;
using System;
using System.Collections.Generic;

namespace Game.State.Root
{
    [Serializable]  // Для сохранения состояния игры, и упаковывания в json
    public class GameState
    {
        public int GlobalEntityId;
        public int CurrentMapId;        // Чтобы можно было сохранять текущую карту
        public List<MapState> Maps = new();

        public int CreateEntityId() => GlobalEntityId++;
    }
}