using Game.State.GameResources;
using Game.State.Maps;
using System;
using System.Collections.Generic;

namespace Game.State.Root
{
    public class GameState
    {
        private int _globalEntityId { get; set; }

        public int CurrentMapId { get; set; }        // Чтобы можно было сохранять текущую карту
        public List<MapData> Maps { get; set; }
        public List<GameResourceData> GameResources { get; set; }

        public int CreateEntityId() => _globalEntityId++;
    }
}