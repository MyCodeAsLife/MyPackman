using R3;

namespace Game.State.GameResources
{
    public class GameResource
    {
        public readonly GameResourceData Origin;
        public readonly ReactiveProperty<int> Amount;

        public GameResource(GameResourceData data)
        {
            Origin = data;
            Amount = new ReactiveProperty<int>(data.Amount);
            Amount.Subscribe(newValue => data.Amount = newValue);
        }

        public GameResourceType GameResourceType => Origin.GameResourceType;
    }
}
