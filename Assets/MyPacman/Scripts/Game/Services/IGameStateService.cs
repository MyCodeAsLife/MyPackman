using R3;

namespace MyPacman
{
    // Интерфейс для провайдеров загрузки\сохранения (в файл, в облако, в базу данных и т.д.)
    public interface IGameStateService
    {
        public GameState GameState { get; }
        public GameSettingsState SettingsState { get; }
        public bool GameStateIsLoaded { get; }

        // Возвращают Observable чтобы загрузку\сохранение можно было подождать.
        public Observable<GameState> LoadGameState();
        public Observable<GameSettingsState> LoadSettingsState();
        public Observable<bool> SaveGameState();
        public Observable<bool> SaveSettingsState();
        public Observable<bool> ResetGameState();
        public Observable<bool> ResetSettingsState();
    }
}