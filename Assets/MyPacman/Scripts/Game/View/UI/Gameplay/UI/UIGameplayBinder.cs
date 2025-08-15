using R3;
using TMPro;
using UnityEngine;
using System.Collections;

namespace MyPacman
{
    public class UIGameplayBinder : WindowBinder<UIGameplayViewModel>
    {
        [SerializeField] private TextMeshProUGUI _highScore;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private GameObject _lifeUpText;

        // Добавить 2 массива картинок для нижней панели
        // 1. для отображения кол-ва жизней
        // 2. для отображения последних поднятых фруктов

        // Здесь передаем на UI изменения из GameState (очки, жизни, время и т.д.)
        protected override void OnBind()
        {
            ViewModel.HighScore.Subscribe(highScore => _highScore.text = highScore.ToString());
            ViewModel.Score.Subscribe(score => _score.text = score.ToString());
            ViewModel.LifePoints.Skip(1).Subscribe(_ => Coroutines.StartRoutine(LifeUpShowing(4f)));        // Magic
        }

        private IEnumerator LifeUpShowing(float duration)
        {
            float timer = 0f;
            float timeDelay = 0.7f;                                                                 // Magic
            var delay = new WaitForSeconds(timeDelay);

            while (timer < duration)
            {
                yield return delay;
                _lifeUpText.SetActive(false);
                yield return delay;
                _lifeUpText.SetActive(true);
                timer += Time.deltaTime;
            }
        }
    }
}
