using R3;
using TMPro;

namespace MyPacman
{
    public interface IUIGameplayViewModel
    {
        public ReadOnlyReactiveProperty<int> HighScore { get; }
        public ReadOnlyReactiveProperty<int> Score { get; }
        public ReadOnlyReactiveProperty<int> LifePoints { get; }

        //public Transform PanelOfRecentlyPickedFruits { get; }
        //public Transform LifeDisplayPanel { get; }
        public TextMeshProUGUI LifeUpText { get; }

        public string Id { get; }
    }
}
