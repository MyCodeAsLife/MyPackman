using R3;
using UnityEngine;

namespace MyPacman
{
    public class PacmanBinder : EntityBinder
    {
        private Rigidbody2D _rigidbody;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void Bind(EntityViewModel viewModel)
        {
            var pacmanViewModel = viewModel as PacmanViewModel;
            // Функция/лямбда(подписка) на поворот в сторону движения.

            pacmanViewModel.Position.Subscribe(nextPosition => _rigidbody.position = nextPosition); // Функция/лямбда(подписка) на движение/смену позиции.
            pacmanViewModel.PassPositionRequestFunction(GetCurrentPosition);
        }

        // Получать позицию игрока здесь, она напрямую зависит от коллайдера и rigitbody
        private Vector2 GetCurrentPosition() => _rigidbody.position;
    }
}
