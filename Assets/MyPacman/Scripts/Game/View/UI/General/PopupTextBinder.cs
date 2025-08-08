using TMPro;
using UnityEngine;

namespace MyPacman
{
    public abstract class PopupTextBinder<T> : MonoBehaviour, IPopupTextBinder where T : PopupTextViewModel
    {
        protected T ViewModel;

        [SerializeField] protected TextMeshPro _textMeshPro;

        public void Bind(PopupTextViewModel viewModel)
        {
            ViewModel = (T)viewModel;
            OnBind(ViewModel);
        }

        public virtual void Close()
        {
            // Здесь мы сначало будем уничтожать, а потом можно делать анимации на закрытие.
            Destroy(gameObject);
        }

        protected virtual void OnBind(PopupTextViewModel viewModel) { }
    }
}
