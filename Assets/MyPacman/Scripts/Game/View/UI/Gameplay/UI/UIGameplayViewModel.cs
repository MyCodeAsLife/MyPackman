using R3;

namespace MyPacman
{
    public class UIGameplayViewModel : UIViewModel
    {
        public ReadOnlyReactiveProperty<int> HighScore;
        public ReadOnlyReactiveProperty<int> Score;
        public ReadOnlyReactiveProperty<int> LifePoints;

        public UIGameplayViewModel(IGameStateService gameState)
        {
            // Определить где и как будет сохранятся максимальный счет, и соответственно откуда будет сюда подцеплятся
            HighScore = gameState.GameState.Score;      // Определится с максимальным счетом
            Score = gameState.GameState.Score;
            LifePoints = gameState.GameState.LifePoints;
        }

        public override string Id => "GameplayUI";                          //Magic
    }
}
