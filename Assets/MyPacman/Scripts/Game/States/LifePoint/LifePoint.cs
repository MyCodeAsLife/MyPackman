using R3;

namespace MyPacman
{
    public class LifePoint
    {
        public readonly LifePointData Origin;
        public readonly ReactiveProperty<int> Amount;

        public LifePoint(LifePointData data)
        {
            Origin = data;
            Amount = new ReactiveProperty<int>(data.Amount);

            Amount.Subscribe(value => data.Amount = value);
        }
    }
}
