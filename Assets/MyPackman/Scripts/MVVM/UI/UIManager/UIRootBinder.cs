using ObservableCollections;
using R3;
using UnityEngine;

namespace MVVM.UI.UIManager
{
    public class UIRootBinder : MonoBehaviour
    {
        private readonly CompositeDisposable _subscriptions = new();

        [SerializeField] private WindowsContainer _windowsContainer;

        private void OnDestroy()
        {
            _subscriptions.Dispose();
        }

        public void Bind(UIRootViewModel viewModel)
        {
            _subscriptions.Add(viewModel.OpenedScreen.Subscribe(newScreenViewModel =>
            {
                _windowsContainer.OpenScreen(newScreenViewModel);
            }));

            foreach (var openedPopup in viewModel.OpenedPopups)
            {
                _windowsContainer.OpenPopup(openedPopup);
            }

            _subscriptions.Add(viewModel.OpenedPopups.ObserveAdd().Subscribe(e =>
            {
                _windowsContainer.OpenPopup(e.Value);
            }));

            _subscriptions.Add(viewModel.OpenedPopups.ObserveRemove().Subscribe(e =>
            {
                _windowsContainer.ClosePopup(e.Value);
            }));

            OnBind(viewModel);
        }

        // Заглушка. Если понадобится сделать какието действя по окончанию привязки
        protected virtual void OnBind(UIRootViewModel viewModel) { }
    }
}