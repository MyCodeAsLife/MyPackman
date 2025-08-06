using ObservableCollections;

namespace MyPacman
{
    public class UIGameplayRootViewModel : UIRootViewModel
    {
        public IObservableCollection<PopupTextViewModel> OpenedPopupTexts => _openedPopupTexts;    //new Отдаем данные только для чтения
        private readonly ObservableList<PopupTextViewModel> _openedPopupTexts = new();              // new

        public UIGameplayRootViewModel()
        {
            // Делаем свои кастомные штучки для сцены, если надо.
        }

        public override void Dispose()
        {
            CloseAllPopupTexts();
            base.Dispose();
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
