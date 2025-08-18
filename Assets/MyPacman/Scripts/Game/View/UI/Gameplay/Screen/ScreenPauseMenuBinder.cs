using UnityEngine;
using UnityEngine.UI;

namespace MyPacman
{
    public class ScreenPauseMenuBinder : WindowBinder<ScreenPauseMenuViewModel>
    {
        [SerializeField] private Button _btnBackgroundCloseWnd;
        [SerializeField] private Button _btnCloseWnd;
        [SerializeField] private Button _btnGoToMainMenu;

        private void OnEnable()
        {
            _btnBackgroundCloseWnd?.onClick.AddListener(OnCloseWndButtonClicked);
            _btnCloseWnd?.onClick.AddListener(OnCloseWndButtonClicked);
            _btnGoToMainMenu?.onClick.AddListener(OnGoToMainMenuButtonClicked);
        }

        private void OnDisable()
        {
            _btnBackgroundCloseWnd?.onClick.RemoveListener(OnCloseWndButtonClicked);
            _btnCloseWnd?.onClick.RemoveListener(OnCloseWndButtonClicked);
            _btnGoToMainMenu?.onClick.RemoveListener(OnGoToMainMenuButtonClicked);
        }

        private void OnCloseWndButtonClicked()
        {
            ViewModel.RequestClose();
        }

        private void OnGoToMainMenuButtonClicked()
        {
            ViewModel.RequestGoToMainMenu();
        }
    }
}
