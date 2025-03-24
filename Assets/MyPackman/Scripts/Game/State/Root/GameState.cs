using Game.State.Buildings;
using System;
using System.Collections.Generic;

namespace Game.State.Root
{
    [Serializable]  // ƒл€ сохранени€ состо€ни€ игры, и упаковывани€ в json
    public class GameState
    {
        public List<BuildingEntity> Buildings = new();
    }
}