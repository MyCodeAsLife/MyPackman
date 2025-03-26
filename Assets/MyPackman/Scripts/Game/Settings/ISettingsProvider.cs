using System.Threading.Tasks;

namespace Game.Settings
{
    public interface ISettingsProvider
    {
        GameSettings GameSettings { get; }                       // Могут загружатся откуда угодно
        ApplicationSettings ApplicationSettings { get; }     // Необходима моментальная загрузка

        Task<GameSettings> LoadGameSettings();      // Асинхронная\многопоточная загрузка настроек
    }
}
