using R3;
using UnityEngine;

namespace MyPacman
{
    public class FruitBinder : EntityBinder
    {
        private static int IsFlashing = Animator.StringToHash(nameof(IsFlashing));    // Именование? Создать отдельные static и вынести туда?

        [SerializeField] private Animator _fruitAnimator;
        [SerializeField] private SpriteRenderer _body;

        private bool _bodyLastState;

        public override void Bind(EntityViewModel viewModel)
        {
            base.Bind(viewModel);
            var fruitViewModel = viewModel as FruitViewModel;

            fruitViewModel.IsFlashing.Subscribe(value => _fruitAnimator.SetBool(IsFlashing, value));
            fruitViewModel.PassFuncHideGhost(HideGhost);
            fruitViewModel.PassFuncShowGhost(ShowGhost);
        }

        private void HideGhost()
        {
            _bodyLastState = _body.enabled;
            _body.enabled = false;
        }

        private void ShowGhost()
        {
            _body.enabled = _bodyLastState;
        }
    }
}
