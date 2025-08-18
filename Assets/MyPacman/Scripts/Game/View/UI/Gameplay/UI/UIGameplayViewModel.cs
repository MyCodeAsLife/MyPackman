using R3;

namespace MyPacman
{
    public class UIGameplayViewModel : UIViewModel
    {
        public readonly ReadOnlyReactiveProperty<int> HighScore;
        public readonly ReadOnlyReactiveProperty<int> Score;
        public readonly ReadOnlyReactiveProperty<int> LifePoints;

        // Потенциальная проблема, в момент создания будет попытка его забиндить с префабом, а поля еще не созданны!!!!!!!
        public UIGameplayViewModel(GameState gameState)     // Не будет ли проблем с отложеной инициализацией?
        {
            // Определить где и как будет сохранятся максимальный счет, и соответственно откуда будет сюда подцеплятся
            HighScore = gameState.Score;      // Определится с максимальным счетом
            Score = gameState.Score;
            LifePoints = gameState.LifePoints;
        }

        public override string Id => "UIGameplay";                          //Magic
    }
}
