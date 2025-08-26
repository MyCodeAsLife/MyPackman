using R3;
using System;
using UnityEngine;

namespace MyPacman
{
    public interface IUIGameplayViewModel
    {
        public ReadOnlyReactiveProperty<int> HighScore { get; }
        public ReadOnlyReactiveProperty<int> Score { get; }

        public Action<bool> SetActiveLifeUpText { get; }       // Передавать через интерфейс только get
        public Transform PanelOfRecentlyPickedFruits { get; }  // Передавать через интерфейс только get
        public Transform LifeDisplayPanel { get; }             // Передавать через интерфейс только get

        public string Id { get; }
    }
}
