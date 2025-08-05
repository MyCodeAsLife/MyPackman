using R3;
using System.Collections;
using UnityEngine;

namespace MyPacman
{
    // Всю догику вынести, оставить только прием спрайтов для установки глаз и переключения аниматоров тела и смена цвета
    // Сохранить цвет?
    public class GhostBinder : EntityBinder
    {
        private const string BehaviorModeType = nameof(BehaviorModeType);

        [SerializeField] private Animator _animatorBody;
        [SerializeField] private SpriteRenderer _eyes;

        // For test
        [SerializeField] private int _lastState;
        [SerializeField] private int _currentState;
        [SerializeField] private string _path;

        private Sprite[] _eyesAll = new Sprite[4];

        public override void Bind(EntityViewModel viewModel)
        {
            base.Bind(viewModel);
            var ghostViewModel = viewModel as GhostViewModel;
            LoadSprites();

            //// Добавить функцию/лямбду(подписка) на поворот в сторону движения.
            ghostViewModel.Direction.Subscribe(direction =>
            {
                // Повернуть
                ChangeDirection(direction);
            });

            //_eyes.enabled = true;             // Выкл злаза в режиме страха
            //ghostViewModel.IsMoving.Subscribe(isMoving => _animator.SetBool(IsMoving, isMoving));  // Переключение анимации движения

            ghostViewModel.Position.Subscribe(nextPosition => transform.position = nextPosition); // Функция/лямбда(подписка) на движение/смену позиции.


            // For test
            _path = viewModel.PrefabPath;
            StartCoroutine(RandomSwitching());
        }

        private void ChangeDirection(Vector2 direction)
        {
            if (direction == Vector2.down)
                _eyes.sprite = _eyesAll[0];
            else if (direction == Vector2.left)
                _eyes.sprite = _eyesAll[1];
            else if (direction == Vector2.right)
                _eyes.sprite = _eyesAll[2];
            else if (direction == Vector2.up)
                _eyes.sprite = _eyesAll[3];

            throw new System.Exception($"Unknown direction: {direction}");
        }

        private void LoadSprites()
        {
            var eyes = Resources.LoadAll<Sprite>("Assets/Sprites/Eyes/");

            for (int i = 0; i < eyes.Length; i++)
            {
                _eyesAll[i] = eyes[i];
            }
        }

        // For test
        // Нормальное переключение между состояниями
        private IEnumerator RandomSwitching()
        {
            _lastState = 0;

            while (true)
            {
                float delay = Random.Range(0f, 3f);
                yield return new WaitForSeconds(delay);
                _currentState = Random.Range(0, 4);

                if (_currentState == 1)
                    continue;

                if (_lastState == 2) // Если был Страх
                {
                    if (_currentState == 0 || _currentState == 3)       // Преследование
                    {
                        _eyes.enabled = true;
                        _animatorBody.SetInteger(BehaviorModeType, _currentState);
                        _lastState = _currentState;
                    }
                }
                else
                {
                    if (_currentState == 2 && _lastState != 3) // Страх
                    {
                        _eyes.enabled = false;
                        _animatorBody.SetInteger(BehaviorModeType, _currentState);
                        _lastState = _currentState;
                    }
                    else if (_currentState != 2)  // Возвращение домой
                    {
                        _eyes.enabled = true;
                        _animatorBody.SetInteger(BehaviorModeType, _currentState);
                        _lastState = _currentState;
                    }
                }

            }

            // 0 - Преследование
            // 2 - Страх
            // 3 - Возвращение домой
        }
    }
}
