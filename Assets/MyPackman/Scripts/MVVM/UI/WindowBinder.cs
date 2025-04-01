using UnityEngine;

namespace MVVM.UI
{
    // IWindowBinder интерфейс необходим для последующего хранения "Биндеров" в списке. Потому как дженерики нельзя хранить в списке
    // Дженерик монобех, чтобы его могли наследовать только потомки WindowViewModel
    public abstract class WindowBinder<T> : MonoBehaviour, IWindowBinder where T : WindowViewModel
    {
        protected T ViewModel;

        public void Bind(WindowViewModel viewModel)
        {
            ViewModel = (T)viewModel;
            OnBind(ViewModel);  // Зачем тут передовать ViewModel потомку, если у него итак есть к ней доступ?
        }

        public virtual void Close()
        {
            //Здесь мы сначало будем уничтожать, а затем можно делать анимации на закрытие.
            Destroy(gameObject);
        }
        protected virtual void OnBind(T viewModel) { }
    }
}
