using R3;

namespace Game.State.Entities.Mergeable
{
    public class MergeableEntity : Entity
    {
        public readonly ReactiveProperty<int> Level;

        public MergeableEntity(MergeableEntityData data) : base(data)
        {
            Level = new ReactiveProperty<int>(data.Level);
            Level.Subscribe(newValue => data.Level = newValue);
        }
    }
}
