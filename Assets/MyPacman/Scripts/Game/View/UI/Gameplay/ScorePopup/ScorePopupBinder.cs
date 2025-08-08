using R3;

namespace MyPacman
{
    public class ScorePopupBinder : PopupTextBinder<ScorePopupTextViewModel>
    {
        protected void OnBind(ScorePopupTextViewModel viewModel)
        {
            base.OnBind(viewModel);

            viewModel.Position.Subscribe(newPos => transform.position = newPos);
            viewModel.Text.Subscribe(newText => _textMeshPro.text = newText);
        }
    }
}
