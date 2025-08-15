using R3;
using TMPro;

namespace MyPacman
{
    public abstract class PopupTextBinder<T> : WindowBinder<T> where T : PopupTextViewModel
    {
        protected TextMeshProUGUI PopupText;

        protected override void OnBind()
        {
            PopupText = GetComponentInChildren<TextMeshProUGUI>();
            ViewModel.Text.Subscribe(newText => PopupText.text = newText);
            ViewModel.Position.Subscribe(newPos => transform.position = newPos);
            ViewModel.TextColor.Skip(1).Subscribe(textColor => PopupText.color = textColor);
        }
    }
}
