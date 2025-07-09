using R3;

namespace MyPacman
{
    public class GhostBinder : EntityBinder
    {
        public override void Bind(EntityViewModel viewModel)
        {
            base.Bind(viewModel);
            var ghostViewModel = viewModel as GhostViewModel;

            //// Добавить функцию/лямбду(подписка) на поворот в сторону движения.
            //ghostViewModel.Direction.Skip(1).Subscribe(direction =>
            //{
            //    // Повернуть
            //    _animator.transform.rotation = Quaternion.Euler(DefineAngle(direction));
            //});

            //ghostViewModel.IsMoving.Subscribe(isMoving => _animator.SetBool(IsMoving, isMoving));  // Переключение анимации движения

            ghostViewModel.Position.Subscribe(nextPosition => transform.position = nextPosition); // Функция/лямбда(подписка) на движение/смену позиции.
        }
    }
}
