using MVVM.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.ScreenGameplay
{
    public class ScreenGameplayBinder : WindowBinder<ScreenGameplayViewModel>
    {
        [SerializeField] private Button _btnPopupA;
        [SerializeField] private Button _btnPopupB;
        [SerializeField] private Button _btnGoToMainMenu;

        private void OnEnable()
        {
            _btnPopupA.onClick.AddListener(OnPopupAButtonClicked);
            _btnPopupB.onClick.AddListener(OnPopupBButtonClicked);
            _btnGoToMainMenu.onClick.AddListener(OnGoToMainMenuButtonClicked);
        }

        private void OnDisable()
        {
            _btnPopupA.onClick.RemoveListener(OnPopupAButtonClicked);
            _btnPopupB.onClick.RemoveListener(OnPopupBButtonClicked);
            _btnGoToMainMenu.onClick.RemoveListener(OnGoToMainMenuButtonClicked);
        }

        private void OnGoToMainMenuButtonClicked()
        {
            ViewModel.RequestOpenPopupA();
        }

        private void OnPopupBButtonClicked()
        {
            ViewModel.RequestOpenPopupB();
        }

        private void OnPopupAButtonClicked()
        {
            ViewModel.RequestGoToMainMenu();
        }
    }
}
