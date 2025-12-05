using System;
using System.Collections;
using UnityEngine;

namespace MyPacman
{
    public class ScoringService
    {
        private readonly GameState _gameState;
        private readonly IUIGameplayViewModel _uiGameplay;

        private int _lifePointCounter;          // Переименовать
        // For test
        private Coroutine _lifeUpShowing;

        public event Action<int, Vector2> PointsReceived;            // для подписи сервиса который будет создавать\уничтожать view c сообщениями на экране

        public ScoringService(
            GameState gameState,
            PickableEntityHandler pickableEntityHandler,
            EntitiesStateHandler entitiesStateHandler,
            IUIGameplayViewModel uiGameplay)
        {
            _gameState = gameState;
            _uiGameplay = uiGameplay;
            _lifePointCounter = _gameState.Map.Value.ScoreForRound.Value / GameConstants.PriceLifePoint;        // Производить это при запуске уровня

            pickableEntityHandler.EntityEaten += OnEntityEaten;
            entitiesStateHandler.EntityEaten += OnEntityEaten;
        }

        private void OnEntityEaten(int points, Vector2 position)
        {
            _gameState.Score.Value += points;
            _gameState.Map.Value.ScoreForRound.Value += points;
            PointsReceived?.Invoke(points, position);

            CheckTheRequiredConditions();
        }

        private void CheckTheRequiredConditions()
        {
            int newNum = _gameState.Map.Value.ScoreForRound.Value / GameConstants.PriceLifePoint;

            if (newNum > _lifePointCounter)
            {
                _lifePointCounter++;
                _gameState.LifePoints.Value++;
                //For test
                _lifeUpShowing = Coroutines.StartRoutine(LifeUpFlickering(5f));     // Magic
            }
        }
        // Мигание отображения кол-ва жизней на UI
        private IEnumerator LifeUpFlickering(float duration)           // Вынести логику в GameplayUIManager и подписать на _gameState.LifePoints.Value++ ??
        {
            float timer = 0f;
            float timeDelay = 0.4f;                                                                 // Magic
            var delay = new WaitForSeconds(timeDelay);
            string text = _uiGameplay.LifeUpText.text;

            while (timer < duration)
            {
                yield return delay;
                _uiGameplay.LifeUpText.enabled = false;

                yield return delay;
                _uiGameplay.LifeUpText.enabled = true;
                timer += (timeDelay * 2);
            }

            _lifeUpShowing = null;
        }
    }
}