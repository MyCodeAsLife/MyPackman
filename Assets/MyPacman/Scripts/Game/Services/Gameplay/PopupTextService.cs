using System.Collections;
using TMPro;
using UnityEngine;

namespace MyPacman
{
    public class PopupTextService
    {
        private readonly ScoringService _scoringService;
        private readonly TextMeshPro _popupText;

        public PopupTextService(ScoringService scoringService)
        {
            _scoringService = scoringService;
            var popupText = Resources.Load<TextMeshPro>(GameConstants.PopupText);

            // For test

            _popupText = Instantiate()
            Coroutines.StartRoutine(TestTextChanged());
        }

        private IEnumerator TestTextChanged()
        {
            float delay = Random.Range(2f, 3f);

            while (true)
            {
                _popupText.text = Random.Range(10, 10001).ToString();
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
