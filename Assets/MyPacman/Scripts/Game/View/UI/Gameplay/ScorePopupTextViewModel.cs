using R3;

namespace MyPacman
{
    public class ScorePopupTextViewModel : PopupTextViewModel
    {
        public ReactiveProperty<int> Score = new ReactiveProperty<int>();

        public override string Id => "ScorePopup";


        public ScorePopupTextViewModel(int score = 0)
        {
            Score.OnNext(score);
        }

        public void ChangeScore(int score)
        {
            Score.OnNext(score);
        }
    }
}
