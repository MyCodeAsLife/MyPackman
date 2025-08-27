using R3;
using TMPro;
using System;
using UnityEngine;

namespace MyPacman
{
    public interface IUIGameplayViewModel
    {
        public ReadOnlyReactiveProperty<int> HighScore { get; }
        public ReadOnlyReactiveProperty<int> Score { get; }

        public Transform PanelOfRecentlyPickedFruits { get; }
        public Transform LifeDisplayPanel { get; }
        public TextMeshProUGUI LifeUpText { get; }

        public string Id { get; }
    }
}
