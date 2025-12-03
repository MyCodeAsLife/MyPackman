using R3;
using UnityEngine;

namespace MyPacman
{
    public class FruitBinder : EntityBinder
    {
        private static int IsFlashing = Animator.StringToHash(nameof(IsFlashing));    // Именование? Создать отдельные static и вынести туда?

        [SerializeField] private Animator _fruitAnimator;

        public override void Bind(EntityViewModel viewModel)
        {
            base.Bind(viewModel);
            var fruitViewModel = viewModel as FruitViewModel;

            fruitViewModel.IsFlashing.Subscribe(value => _fruitAnimator.SetBool(IsFlashing, value));
        }
    }
}
