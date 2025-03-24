using Game.State.Buildings;
using Game.State.Root;
using R3;
using System.Collections.Generic;
using UnityEngine;

namespace Game.State
{
    public class PlayerPrefsGameStateProvider : IGameStateProvider
    {
        private const string GAME_STATE_KEY = nameof(GAME_STATE_KEY);   // Вынести в константы?

        public GameStateProxy GameState { get; private set; }

        private GameState _gameStateOrigin;

        public Observable<GameStateProxy> LoadGameState()
        {
            // Если ключа нет(если сохранения нет), то создаем дефолтное состояние
            if (!PlayerPrefs.HasKey(GAME_STATE_KEY))
            {
                GameState = CreateGameStateFromSettings();
                //for tests
                Debug.Log("Game State created from settings: " + JsonUtility.ToJson(_gameStateOrigin, true));
                // Сохраняем дефолтное состояние
                SaveGameState();
            }
            else  // Если ключ есть, то загружаем состояние
            {
                var json = PlayerPrefs.GetString(GAME_STATE_KEY);
                _gameStateOrigin = JsonUtility.FromJson<GameState>(json);
                GameState = new GameStateProxy(_gameStateOrigin);
            }

            return Observable.Return(GameState);    // Возвращаемый объект позволяет ждать завершения процесса
        }

        public Observable<bool> SaveGameState()
        {
            var json = JsonUtility.ToJson(_gameStateOrigin, true);
            PlayerPrefs.SetString(GAME_STATE_KEY, json);

            return Observable.Return(true);     // Возвращаемый объект позволяет ждать завершения процесса
        }

        private GameStateProxy CreateGameStateFromSettings()
        {
            // Симулируем дефолтные предустановки
            _gameStateOrigin = new GameState
            {
                Buildings = new List<BuildingEntity>
                {
                    new()
                    {
                        TypeId = "PRO100"
                    },
                    new()
                    {
                        TypeId = "SEMA4"
                    }
                }
            };

            return new GameStateProxy(_gameStateOrigin);
        }

        public Observable<bool> ResetGameState()
        {
            GameState = CreateGameStateFromSettings();
            SaveGameState();

            return Observable.Return(true); // Возвращаемый объект позволяет ждать завершения процесса
        }
    }
}
