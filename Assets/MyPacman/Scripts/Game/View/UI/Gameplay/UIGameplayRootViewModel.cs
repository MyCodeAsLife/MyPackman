using ObservableCollections;
using R3;

namespace MyPacman
{
    public class UIGameplayRootViewModel : UIRootViewModel
    {
        public IObservableCollection<PopupTextViewModel> OpenedPopupTexts => _openedPopupTexts;    //new Отдаем данные только для чтения
        public ReadOnlyReactiveProperty<UIGameplayViewModel> GameplayUI => _gameplayUI;

        private readonly ObservableList<PopupTextViewModel> _openedPopupTexts = new();             // new
        private readonly ReactiveProperty<UIGameplayViewModel> _gameplayUI = new();

        public UIGameplayRootViewModel()
        {
            // Делаем свои кастомные штучки для сцены, если надо.
        }

        public override void Dispose()
        {
            CloseAllPopupTexts();
            base.Dispose();
        }

        public void CreateGameplayUI(UIGameplayViewModel gameplayUI)
        {
            _gameplayUI.Value?.Dispose();
            _gameplayUI.Value = gameplayUI;
        }

        public void OpenPopupText(PopupTextViewModel popupViewModel)
        {
            if (_openedPopupTexts.Contains(popupViewModel))
                return;

            _openedPopupTexts.Add(popupViewModel);
        }

        public void ClosePopupText(PopupTextViewModel popupViewModel)
        {
            if (_openedPopupTexts.Contains(popupViewModel))
            {
                popupViewModel.Dispose();
                _openedPopupTexts.Remove(popupViewModel);
            }
        }

        public void CloseAllPopupTexts()
        {
            foreach (var popup in _openedPopupTexts)
                ClosePopupText(popup);
        }
    }
}
