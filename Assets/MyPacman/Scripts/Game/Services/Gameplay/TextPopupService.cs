using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    public class TextPopupService
    {
        private readonly GameplayUIManager _uiManager;
        private List<ScorePopupTextViewModel> _popupTexts = new();

        public TextPopupService(GameplayUIManager uiManager, ScoringService scoringService)
        {
            _uiManager = uiManager;
            scoringService.PointsReceived += OnPointsReceived;
        }

        private void OnPointsReceived(int score, Vector2 position)
        {
            var popup = _uiManager.OpenScorePopupText();
            _popupTexts.Add(popup);
            string text = score.ToString() + '!';                                           // Magic
            popup.SetText(text);
            popup.SetPosition(position);
            Coroutines.StartRoutine(ShowingPopupText(popup, position, 1f));           // Magic
        }

        private IEnumerator ShowingPopupText(ScorePopupTextViewModel scorePopup, Vector2 position, float duration)
        {
            float timer = 0f;
            float alpha = 1f;
            float rateOfChange = 1 / duration;
            Color textColor = Color.white;

            while (timer < duration)
            {
                yield return null;
                timer += Time.deltaTime;

                alpha = Mathf.MoveTowards(alpha, 0f, rateOfChange * Time.deltaTime);
                textColor = new Color(textColor.r, textColor.g, textColor.b, alpha);
                scorePopup.SetColor(textColor);

                Vector2 nextPos = new Vector2(position.x + timer * 5, position.y + timer * 5);
                scorePopup.SetPosition(nextPos);
            }

            _popupTexts.Remove(scorePopup);
            _uiManager.CloseScorePopupText(scorePopup);
        }
    }
}
