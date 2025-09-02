using ObservableCollections;
using R3;
using TMPro;

namespace MyPacman
{
    public class UIGameplayViewModel : UIViewModel, IUIGameplayViewModel
    {
        private readonly ReadOnlyReactiveProperty<int> _highScore;
        private readonly ReadOnlyReactiveProperty<int> _score;
        private readonly ReadOnlyReactiveProperty<int> _lifePoints;
        private readonly IReadOnlyObservableList<EntityType> _pickedFruits;

        public UIGameplayViewModel(GameState gameState)     // Передать не весь объект а конкретные поля
        {
            // Определить где и как будет сохранятся максимальный счет, и соответственно откуда будет сюда подцеплятся
            _highScore = gameState.Score;      // Определится с максимальным счетом
            _score = gameState.Score;
            _lifePoints = gameState.LifePoints;
            _pickedFruits = gameState.PickedFruits;
        }

        public override string Id => "UIGameplay";                  //Magic
        public ReadOnlyReactiveProperty<int> HighScore => _highScore;
        public ReadOnlyReactiveProperty<int> Score => _score;
        public ReadOnlyReactiveProperty<int> LifePoints => _lifePoints;
        public IReadOnlyObservableList<EntityType> PickedFruits => _pickedFruits;
        public TextMeshProUGUI LifeUpText { get; set; }

    }
}
