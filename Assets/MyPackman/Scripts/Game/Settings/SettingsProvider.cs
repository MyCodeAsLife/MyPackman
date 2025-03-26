using System.Threading.Tasks;
using UnityEngine;

namespace Game.Settings
{
    public class SettingsProvider : ISettingsProvider
    {
        private GameSettings _gameSettings;

        public SettingsProvider()
        {
            ApplicationSettings = Resources.Load<ApplicationSettings>("Settings/ApplicationSettings");
        }

        public GameSettings GameSettings => _gameSettings;

        public ApplicationSettings ApplicationSettings { get; }

        public Task<GameSettings> LoadGameSettings()
        {
            _gameSettings = Resources.Load<GameSettings>("Settings/GameSettings");
            return Task.FromResult(GameSettings);
        }
    }
}
