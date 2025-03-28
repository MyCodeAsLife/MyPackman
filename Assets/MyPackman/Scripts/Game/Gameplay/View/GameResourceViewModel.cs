using Game.State.GameResources;
using R3;

namespace Game.Gameplay.View
{
    public class GameResourceViewModel
    {
        public readonly GameResourceType GameResourceType;
        public readonly ReadOnlyReactiveProperty<int> Amount;

        public GameResourceViewModel(GameResource resource)
        {
            GameResourceType = resource.GameResourceType;
            Amount = resource.Amount;
        }
    }
}
