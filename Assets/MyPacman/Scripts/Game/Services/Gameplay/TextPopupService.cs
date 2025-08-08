using System.Collections;
using UnityEngine;

namespace MyPacman
{
    public class TextPopupService
    {
        private readonly GameplayUIManager _uiManager;
        //private readonly Dictionary<int, ScorePopupTextViewModel> _textPopups = new();

        //private readonly Func<int> _getId;      // Принцип именования

        public TextPopupService(GameplayUIManager uiManager, ScoringService scoringService/*, Func<int> getId*/)
        {
            _uiManager = uiManager;
            //_getId = getId;
            scoringService.PointsReceived += OnPointsReceived;
        }

        private void OnPointsReceived(int score, Vector2 position)
        {
            var popup = _uiManager.OpenScorePopup();
            //_textPopups.Add(popup.Id, popup);
            string text = score.ToString() + '!';                                           // Magic

            Coroutines.StartRoutine(TextDisplayTimer(popup, text, 2f));                     // Magic
        }

        private IEnumerator TextDisplayTimer(ScorePopupTextViewModel scorePopup, string text, float duration)
        {
            float timer = 0f;

            while (true)
            {
                yield return null;

                if (timer < duration)
                    timer += Time.deltaTime;
                else
                    break;

                // "Анимация" - изменение позиции всплывающего текста 
            }

            //_textPopups.Remove(scorePopup.Id);
            _uiManager.CloseScorePopup(scorePopup);
        }
    }
}
