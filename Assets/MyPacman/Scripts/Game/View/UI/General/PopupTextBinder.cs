using R3;
using TMPro;
using UnityEngine;

namespace MyPacman
{
    public abstract class PopupTextBinder<T> : MonoBehaviour, IPopupTextBinder where T : PopupTextViewModel
    {
        protected T ViewModel;
        protected TextMeshProUGUI PopupText;

        public void Bind(PopupTextViewModel viewModel)
        {
            ViewModel = (T)viewModel;
            OnBind(ViewModel);
            PopupText = GetComponentInChildren<TextMeshProUGUI>();
            ViewModel.Text.Subscribe(newText => PopupText.text = newText);
            ViewModel.Position.Subscribe(newPos => transform.position = newPos);
            ViewModel.TextColor.Skip(1).Subscribe(textColor => PopupText.color = textColor);
        }

        public virtual void Close()
        {
            // Здесь мы сначало будем уничтожать, а потом можно делать анимации на закрытие.
            Destroy(gameObject);
        }

        protected virtual void OnBind(PopupTextViewModel viewModel) { }
    }
}
