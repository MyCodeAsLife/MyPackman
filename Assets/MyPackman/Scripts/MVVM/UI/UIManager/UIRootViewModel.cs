using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVM.UI.UIManager
{
    public class UIRootViewModel : IDisposable
    {
        public ReadOnlyReactiveProperty<WindowViewModel> OpenedScreen => _openedScreen;
        public IObservableCollection<WindowViewModel> OpenedPopups => _openedPopups;

        private readonly ReactiveProperty<WindowViewModel> _openedScreen = new(null);
        private readonly ObservableList<WindowViewModel> _openedPopups = new();
        private Dictionary<WindowViewModel, IDisposable> _popupSubscriptions = new();

        public void Dispose()
        {
            CloseAllPopups();
            _openedScreen.Dispose();
        }

        public void OpenScreen(WindowViewModel screenViewModel)
        {
            _openedScreen.Value?.Dispose();
            _openedScreen.Value = screenViewModel;
        }

        public void OpenPopup(WindowViewModel popupViewModel)
        {
            if (_openedPopups.Contains(popupViewModel))
            {
                return;
            }

            var subscriptions = popupViewModel.CloseRequested.Subscribe(viewModel => ClosePopup(viewModel));
            _popupSubscriptions.Add(popupViewModel, subscriptions);
            _openedPopups.Add(popupViewModel);
        }

        public void ClosePopup(WindowViewModel popupViewModel)
        {
            if (_openedPopups.Contains(popupViewModel))
            {
                popupViewModel.Dispose();
                _openedPopups.Remove(popupViewModel);

                var popupSubscription = _popupSubscriptions[popupViewModel];
                popupSubscription?.Dispose();
                _popupSubscriptions.Remove(popupViewModel);
            }

        }

        public void ClosePopup(string popupId)
        {
            var openedPopupViewModel = _openedPopups.FirstOrDefault(p => p.Id == popupId);
            ClosePopup(openedPopupViewModel);
        }

        public void CloseAllPopups()
        {
            foreach (var popup in _openedPopups)
            {
                ClosePopup(popup);
            }
        }
    }
}