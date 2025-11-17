using R3;
using UnityEngine;

namespace MyPacman
{
    public class PacmanBinder : EntityBinder
    {
        private static int Dead = Animator.StringToHash(nameof(Dead));          // Именование? Создать отдельные static и вынести туда?

        private const string IsMoving = nameof(IsMoving);                       // Вынести в константы?

        private Rigidbody2D _rigidbody;
        private Animator _animator;
        // New
        private PacmanAnimationEvents _animationEvents;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponentInChildren<Animator>();
            // New
            _animationEvents = GetComponentInChildren<PacmanAnimationEvents>();
        }

        public override void Bind(EntityViewModel viewModel)
        {
            base.Bind(viewModel);
            var pacmanViewModel = viewModel as PacmanViewModel;

            // Добавить функцию/лямбду(подписка) на поворот в сторону движения.
            pacmanViewModel.Direction.Skip(1).Subscribe(direction =>
            {
                // Повернуть
                _animator.transform.rotation = Quaternion.Euler(direction.DefineAngle());
            });

            pacmanViewModel.IsMoving.Subscribe(isMoving => _animator.SetBool(IsMoving, isMoving));  // Переключение анимации движения
            pacmanViewModel.Position.Subscribe(nextPosition => _rigidbody.position = nextPosition); // Функция/лямбда(подписка) на движение/смену позиции.
            pacmanViewModel.PassPositionRequestFunction(GetCurrentPosition);

            // Вынести в константы и преобразовать в public readonly int Dead = Animator.StringToHash(nameof(Dead));
            pacmanViewModel.Dead.Subscribe(_ => _animator.SetTrigger(Dead));
            pacmanViewModel.SubscribeToDeadAnimationFinish(_animationEvents.DeadAnimationFinish);
        }

        // Получать позицию игрока здесь, она напрямую зависит от коллайдера и rigitbody
        private Vector2 GetCurrentPosition() => _rigidbody.position;
    }
}
