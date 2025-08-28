using R3;
using TMPro;
using UnityEngine;
using ObservableCollections;

namespace MyPacman
{
    public class UIGameplayBinder : WindowBinder<UIGameplayViewModel>
    {
        [SerializeField] private TextMeshProUGUI _highScore;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _lifeUpText;
        [SerializeField] private UIGameplayIconContainer _iconContainer;

        // Здесь передаем на UI изменения из GameState (очки, жизни, время и т.д.)
        protected override void OnBind()
        {
            ViewModel.HighScore.Subscribe(highScore => _highScore.text = highScore.ToString());
            ViewModel.Score.Subscribe(score => _score.text = score.ToString());

            // New
            ViewModel.LifeUpText = _lifeUpText;
            ViewModel.LifePoints.Subscribe(lifePoints => _iconContainer.OnPlayerLifePointsChanged(lifePoints));
            ViewModel.PickedFruits.ObserveRemove().Subscribe(entityType => _iconContainer.HideIcon(entityType.Value));
            ViewModel.PickedFruits.ObserveAdd().Subscribe(entityType => _iconContainer.ShowIcon(entityType.Value));
            // Нужна функция перебора массива PickedFruits и ручного добавления иконок?
        }
    }
}
