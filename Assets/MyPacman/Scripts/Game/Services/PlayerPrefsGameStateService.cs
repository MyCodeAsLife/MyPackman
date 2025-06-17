using Newtonsoft.Json;
using R3;
using UnityEngine;

namespace MyPacman
{
    public class PlayerPrefsGameStateService : IGameStateService
    {
        private const string GAME_STATE_KEY = nameof(GAME_STATE_KEY);           // Вынести в контсанты
        private const string SETTINGS_STATE_KEY = nameof(SETTINGS_STATE_KEY);   // Вынести в контсанты

        public GameState GameState { get; private set; }
        public GameSettingsState SettingsState { get; private set; }

        private GameStateData _gameStateOrigin { get; set; }
        private GameSettingsStateData _settingsStateOrigin { get; set; }

        public Observable<GameState> LoadGameState()   // Похожа на LoadSettingsState
        {
            // Для того чтобы Newtonsoft.JsonConvert нормально десериализовывал объекты с наследованием
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            };

            if (!PlayerPrefs.HasKey(GAME_STATE_KEY))
            {
                GameState = CreateGameStateFromSettings();  // Создаем дефолтное состояние
                                                            // В продакшене Formatting.Indented лучше не использовать, даполнительные затраты ресурсов
                Debug.Log("GameState created from settings: " + JsonConvert.SerializeObject(_gameStateOrigin, Formatting.Indented));    //++++++++++++++++++++++++++++++++

                SaveGameState();    // Сохраняем дефолтное состояние
            }
            else
            {
                // Загружаем
                var json = PlayerPrefs.GetString(GAME_STATE_KEY);
                _gameStateOrigin = JsonConvert.DeserializeObject<GameStateData>(json);
                GameState = new GameState(_gameStateOrigin);

                Debug.Log("GameState loaded: " + json);                                  //++++++++++++++++++++++++++++++++
            }

            return Observable.Return(GameState);
        }

        public Observable<GameSettingsState> LoadSettingsState()   // Похожа на LoadGameState
        {
            if (!PlayerPrefs.HasKey(SETTINGS_STATE_KEY))
            {
                SettingsState = CreateGameSettingsStateFromSettings();  // Создаем дефолтное состояние
                                                                        //Debug.Log("GameSettingsState created from settings: " + JsonUtility.ToJson(_settingsStateOrigin, true));    //++++++++++++++++++++++++++++++++

                SaveSettingsState();    // Сохраняем дефолтное состояние
            }
            else
            {
                // Загружаем
                var json = PlayerPrefs.GetString(SETTINGS_STATE_KEY);
                _settingsStateOrigin = JsonConvert.DeserializeObject<GameSettingsStateData>(json);
                SettingsState = new GameSettingsState(_settingsStateOrigin);

                Debug.Log("GameSettingsState loaded: " + json);                                  //++++++++++++++++++++++++++++++++
            }

            return Observable.Return(SettingsState);
        }

        public Observable<bool> SaveGameState() // Похожа на SaveSettingsState
        {
            var json = JsonConvert.SerializeObject(_gameStateOrigin, Formatting.Indented);
            PlayerPrefs.SetString(GAME_STATE_KEY, json);

            return Observable.Return(true);
        }

        public Observable<bool> SaveSettingsState() // Похожа на SaveGameState
        {
            var json = JsonConvert.SerializeObject(_settingsStateOrigin, Formatting.Indented);
            PlayerPrefs.SetString(SETTINGS_STATE_KEY, json);

            return Observable.Return(true);
        }

        public Observable<bool> ResetGameState()    // Похожа на ResetSettingsState
        {
            GameState = CreateGameStateFromSettings();
            SaveGameState();

            return Observable.Return(true);
        }

        public Observable<bool> ResetSettingsState()    // Похожа на ResetGameState
        {
            SettingsState = CreateGameSettingsStateFromSettings();
            SaveSettingsState();

            return Observable.Return(true);
        }

        // Настройки по умолчанию загружать из выбранной карты
        private GameState CreateGameStateFromSettings()
        {
            //// Делаем фейк
            //_gameStateOrigin = new GameStateData
            //{
            //    Maps = new List<MapData>(),
            //    Resources = new List<ResourceData>()
            //{
            //    new() {ResourceType = ResourceType.SoftCurrency, Amount = 0},
            //    new() {ResourceType = ResourceType.HardCurrency, Amount = 0},
            //}
            //};

            _gameStateOrigin = new GameStateData()
            {
                CurrentMapId = 0,                   // Правильно ли что оно тут инициализируется?
                Map = new MapData() { Entities = new() },
                Score = new ScoreData(),
                HigthScore = new ScoreData() { Amount = 20000 },
                LifePoints = new LifePointData() { Amount = 3 },
            };

            return new GameState(_gameStateOrigin);
        }

        private GameSettingsState CreateGameSettingsStateFromSettings()
        {
            // Делаем фейк
            _settingsStateOrigin = new GameSettingsStateData
            {
                MusicVolume = 22,
                SFXVolume = 33,
            };

            return new GameSettingsState(_settingsStateOrigin);
        }
    }
}