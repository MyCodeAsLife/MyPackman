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
        //[SerializeField] private SpriteRenderer _body;
        [SerializeField] private SpriteRenderer _eyes;

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
        private IEnumerator RandomSwitching()
        {
            int lastState = 0;

            while (true)
            {
                float delay = Random.Range(0f, 3f);
                int randomState = Random.Range(0, 4);

                if (randomState == 3)       // Возвращение домой
                {
                    _eyes.enabled = true;
                    _animatorBody.SetInteger(BehaviorModeType, randomState);
                    lastState = randomState;
                }
                else
                {
                    _animatorBody.SetInteger(BehaviorModeType, randomState);

                    if (randomState == 2 && lastState != 3)   // Страх
                    {
                        _eyes.enabled = false;

                    }
                    else
                    {
                        _eyes.enabled = true;
                    }

                    lastState = randomState;
                }

                yield return new WaitForSeconds(delay);
            }
        }
    }
}
