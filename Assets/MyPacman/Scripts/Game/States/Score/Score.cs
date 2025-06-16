using R3;

namespace MyPacman
{
    public class Score
    {
        public readonly ScoreData Origin;
        public readonly ReactiveProperty<int> Amount;

        public Score(ScoreData data)
        {
            Origin = data;
            Amount = new ReactiveProperty<int>(data.Amount);

            Amount.Subscribe(value => data.Amount = value);
        }
    }
}