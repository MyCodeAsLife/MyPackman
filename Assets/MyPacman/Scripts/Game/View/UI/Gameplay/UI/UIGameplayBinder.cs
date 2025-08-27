using R3;
using TMPro;
using UnityEngine;

namespace MyPacman
{
    public class UIGameplayBinder : WindowBinder<UIGameplayViewModel>
    {
        [SerializeField] private TextMeshProUGUI _highScore;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _lifeUpText;
        [SerializeField] private Transform _panelOfRecentlyPickedFruits;
        [SerializeField] private Transform _lifeDisplayPanel;
        [SerializeField] private UIGameplayIconContainer _iconContainer;

        // Здесь передаем на UI изменения из GameState (очки, жизни, время и т.д.)
        protected override void OnBind()
        {
            ViewModel.HighScore.Subscribe(highScore => _highScore.text = highScore.ToString());
            ViewModel.Score.Subscribe(score => _score.text = score.ToString());
            //ViewModel.LifePoints.Skip(1).Subscribe(_ => Coroutines.StartRoutine(LifeUpShowing(4f)));        // Magic

            // New
            ViewModel.PanelOfRecentlyPickedFruits = _panelOfRecentlyPickedFruits;
            ViewModel.LifeDisplayPanel = _lifeDisplayPanel;
            ViewModel.LifeUpText = _lifeUpText;

            // For test
            _iconContainer.ShowFruits();

            // 1. Перебираем из GameState массив с подобранными фруктами и отправляем сигнал в контейнер на создание иконки
            // 2. Подписываемся на массив с подобранными фруктами
            // 3. Прописать в контейнер ограничение на 10 иконок на панелях, добавить в константы ограничивающее число
        }
    }
}
