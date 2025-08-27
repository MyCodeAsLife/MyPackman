using R3;
using System;
using TMPro;
using UnityEngine;

namespace MyPacman
{
    public class UIGameplayViewModel : UIViewModel, IUIGameplayViewModel
    {
        private readonly ReadOnlyReactiveProperty<int> _highScore;
        private readonly ReadOnlyReactiveProperty<int> _score;
        //public readonly ReadOnlyReactiveProperty<int> LifePoints;       // Убрать после выноса объекта в GameplayUIManager ?

        public UIGameplayViewModel(GameState gameState)     // Передать не весь объект а конкретные поля
        {
            // Определить где и как будет сохранятся максимальный счет, и соответственно откуда будет сюда подцеплятся
            _highScore = gameState.Score;      // Определится с максимальным счетом
            _score = gameState.Score;
            //LifePoints = gameState.LifePoints;
        }

        public override string Id => "UIGameplay";                  //Magic
        public ReadOnlyReactiveProperty<int> HighScore => _highScore;
        public ReadOnlyReactiveProperty<int> Score => _score;
        public Transform PanelOfRecentlyPickedFruits { get; set; }
        public Transform LifeDisplayPanel { get; set; }
        public TextMeshProUGUI LifeUpText {  get; set; } 
    }
}
