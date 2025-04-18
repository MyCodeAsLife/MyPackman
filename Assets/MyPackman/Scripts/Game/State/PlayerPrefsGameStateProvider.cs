﻿using Game.State.GameResources;
using Game.State.Maps;
using Game.State.Root;
using Newtonsoft.Json;
using R3;
using System.Collections.Generic;
using UnityEngine;

namespace Game.State
{
    // Провайдер для сохранения состояний в PlayerPrefs
    public class PlayerPrefsGameStateProvider : IGameStateProvider
    {
        private const string GAME_STATE_KEY = nameof(GAME_STATE_KEY);   // Вынести в константы?
        private const string SETTINGS_STATE_KEY = nameof(SETTINGS_STATE_KEY);   // Вынести в константы?

        public GameStateProxy GameState { get; private set; }
        public GameSettingsStateProxy SettingsState { get; private set; }

        private GameState _gameStateOrigin;
        private GameSettingsState _settingsStateOrigin;

        public Observable<GameStateProxy> LoadGameState()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            };

            // Если ключа нет(если сохранения нет), то создаем дефолтное состояние
            if (!PlayerPrefs.HasKey(GAME_STATE_KEY))
            {
                GameState = CreateGameStateFromSettings();
                //for tests
                Debug.Log("Game State created from settings: "
                    + JsonConvert.SerializeObject(_gameStateOrigin, Formatting.Indented));
                // Сохраняем дефолтное состояние
                SaveGameState();
            }
            else  // Если ключ есть, то загружаем состояние
            {
                var json = PlayerPrefs.GetString(GAME_STATE_KEY);
                _gameStateOrigin = JsonConvert.DeserializeObject<GameState>(json);
                GameState = new GameStateProxy(_gameStateOrigin);
                //for tests
                Debug.Log("Game State loaded from settings: "
                    + JsonConvert.SerializeObject(_gameStateOrigin, Formatting.Indented));
            }

            return Observable.Return(GameState);    // Возвращаемый объект позволяет ждать завершения процесса
        }

        public Observable<GameSettingsStateProxy> LoadSettingsState()
        {
            // Если ключа нет(если сохранения нет), то создаем дефолтное состояние
            if (!PlayerPrefs.HasKey(SETTINGS_STATE_KEY))
            {
                SettingsState = CreateSettingsStateFromSettings();
                //for tests
                Debug.Log("Game Settings State created from settings: "
                    + JsonConvert.SerializeObject(_settingsStateOrigin, Formatting.Indented));
                // Сохраняем дефолтное состояние
                SaveSettingsState();
            }
            else  // Если ключ есть, то загружаем состояние
            {
                var json = PlayerPrefs.GetString(SETTINGS_STATE_KEY);
                _settingsStateOrigin = JsonConvert.DeserializeObject<GameSettingsState>(json);
                SettingsState = new GameSettingsStateProxy(_settingsStateOrigin);
            }

            return Observable.Return(SettingsState);    // Возвращаемый объект позволяет ждать завершения процесса
        }

        private GameStateProxy CreateGameStateFromSettings()
        {
            // Симулируем дефолтные предустановки
            _gameStateOrigin = new GameState
            {
                Maps = new List<MapData>(),
                GameResources = new List<GameResourceData>()
                {
                    new(){Amount = 0, GameResourceType = GameResourceType.SoftCurrency },
                    new(){Amount = 0, GameResourceType = GameResourceType.HardCurrency },
                },
            };

            return new GameStateProxy(_gameStateOrigin);
        }

        private GameSettingsStateProxy CreateSettingsStateFromSettings()
        {
            // Симулируем дефолтные предустановки
            _settingsStateOrigin = new GameSettingsState
            {
                MusicVolume = 60,
                SFXVolume = 74,
            };

            return new GameSettingsStateProxy(_settingsStateOrigin);
        }

        public Observable<bool> SaveGameState()
        {
            var json = JsonConvert.SerializeObject(_gameStateOrigin, Formatting.Indented);
            PlayerPrefs.SetString(GAME_STATE_KEY, json);

            return Observable.Return(true);     // Возвращаемый объект позволяет ждать завершения процесса
        }

        public Observable<bool> SaveSettingsState()
        {
            var json = JsonConvert.SerializeObject(_settingsStateOrigin, Formatting.Indented);
            PlayerPrefs.SetString(SETTINGS_STATE_KEY, json);

            return Observable.Return(true);     // Возвращаемый объект позволяет ждать завершения процесса
        }

        public Observable<bool> ResetGameState()
        {
            GameState = CreateGameStateFromSettings();
            SaveGameState();

            return Observable.Return(true); // Возвращаемый объект позволяет ждать завершения процесса
        }

        public Observable<bool> ResetSettingsState()
        {
            SettingsState = CreateSettingsStateFromSettings();
            SaveSettingsState();

            return Observable.Return(true); // Возвращаемый объект позволяет ждать завершения процесса
        }
    }
}
