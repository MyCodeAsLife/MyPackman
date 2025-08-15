using UnityEngine;

namespace MyPacman
{
    public abstract class WindowBinder<T> : MonoBehaviour, IUIBinder where T : UIViewModel
    {
        protected T ViewModel;

        public void Bind(UIViewModel viewModel)
        {
            ViewModel = (T)viewModel;
            OnBind();
        }

        public virtual void Close()
        {
            // Здесь мы сначало будем уничтожать, а потом можно делать анимации на закрытие.
            Destroy(gameObject);
        }

        protected virtual void OnBind() { }
    }
}
