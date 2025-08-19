using System;
using UnityEngine;

namespace MyPacman
{
    public class ScoringService
    {
        private readonly GameState _gameState;

        private int _scoreForRound;

        public event Action<int, Vector2> PointsReceived;            // для подписи сервиса который будет создавать\уничтожать view c сообщениями на экране

        public ScoringService(GameState gameState, MapHandlerService mapHandlerService)
        {
            _gameState = gameState;
            mapHandlerService.EntityEaten += OnEntityEaten;
        }

        private void OnEntityEaten(EdibleEntityPoints enumPoints, Vector2 position)
        {
            int points = (int)enumPoints;
            _gameState.Score.Value += points;
            _scoreForRound += points;
            PointsReceived?.Invoke(points, position);

            CheckTheRequiredConditions();
        }

        private void CheckTheRequiredConditions()
        {
            if (_scoreForRound >= GameConstants.PriceLifePoint)
                _gameState.LifePoints.Value++;
        }
    }
}