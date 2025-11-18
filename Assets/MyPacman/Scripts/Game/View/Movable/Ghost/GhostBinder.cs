using R3;
using UnityEngine;

namespace MyPacman
{
    // Всю догику вынести, оставить только прием спрайтов для установки глаз и переключения аниматоров тела и смена цвета
    // Сохранить цвет?
    public class GhostBinder : EntityBinder
    {
        //private const string BehaviorModeType = nameof(BehaviorModeType);       // Вынести в const?
        private static int BehaviorModeType = Animator.StringToHash(nameof(BehaviorModeType));    // Именование? Создать отдельные static и вынести туда?

        [SerializeField] private Animator _animatorBody;
        [SerializeField] private SpriteRenderer _eyes;
        [SerializeField] private SpriteRenderer _body;

        // For test
        //[SerializeField] private GhostBehaviorModeType _lastBehaviorModeType;
        //[SerializeField] private GhostBehaviorModeType _currentBehaviorModeType;
        //[SerializeField] private string _path;

        private Sprite[] _eyesAll = new Sprite[4];

        private bool _eyesLastState;
        private bool _bodyLastState;

        public override void Bind(EntityViewModel viewModel)
        {
            base.Bind(viewModel);
            var ghostViewModel = viewModel as GhostViewModel;
            LoadSprites();

            ghostViewModel.Direction.Subscribe(direction =>              // Повернуть глаза
            {
                ChangeDirection(direction);
            });

            ghostViewModel.CurrentBehaviorMode.Subscribe(newType =>     // Если включается режим страха, то отключить глаза
            {
                _eyes.enabled = newType == GhostBehaviorModeType.Frightened ? false : true;
                _animatorBody.SetInteger(BehaviorModeType, (int)newType);
            });

            ghostViewModel.Position.Subscribe(nextPosition => transform.position = nextPosition); // Функция/лямбда(подписка) на движение/смену позиции.
            ghostViewModel.PassFuncHideGhost(HideGhost);
            ghostViewModel.PassFuncShowGhost(ShowGhost);

            //// For test
            //_path = viewModel.PrefabPath;               // Используется?
            //StartCoroutine(RandomSwitching());
        }

        private void HideGhost()
        {
            _eyesLastState = _eyes.enabled;
            _bodyLastState = _body.enabled;

            _eyes.enabled = false;
            _body.enabled = false;
        }

        private void ShowGhost()
        {
            _eyes.enabled = _eyesLastState;
            _body.enabled = _bodyLastState;
        }

        private void ChangeDirection(Vector2 direction) // Вынести логику?
        {
            if (direction == Vector2.down)
                _eyes.sprite = _eyesAll[0];
            else if (direction == Vector2.left)
                _eyes.sprite = _eyesAll[1];
            else if (direction == Vector2.right)
                _eyes.sprite = _eyesAll[2];
            else if (direction == Vector2.up)
                _eyes.sprite = _eyesAll[3];

            throw new System.Exception($"Unknown direction: {direction}");      //Magic
        }

        private void LoadSprites()      // Вынести логику?
        {
            var eyes = Resources.LoadAll<Sprite>("Assets/Sprites/Eyes/");       //Magic

            for (int i = 0; i < eyes.Length; i++)
            {
                _eyesAll[i] = eyes[i];
            }
        }
    }
}