using Game.State.Root;
using R3;

namespace Game.State
{
    // Интерфейс, потому что мы незнаем какой именно у нас будет загрузчик, из облака, с диска и т.д.
    public interface IGameStateProvider
    {
        public GameStateProxy GameState { get; }

        // Возвращает Observable для того чтобы можно было ждать оканчания процесса
        public Observable<GameStateProxy> LoadGameState();
        public Observable<bool> SaveGameState();
        public Observable<bool> ResetGameState();
    }
}
