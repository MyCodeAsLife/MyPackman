using UnityEngine;
using UnityEngine.UI;

namespace MyPacman
{
    public class ScreenMainMenuGameplayBinder : WindowBinder<ScreenMainMenuGameplayViewModel>
    {
        //[SerializeField] private Button _btnPopupA;
        //[SerializeField] private Button _btnPopupB;
        [SerializeField] private Button _btnGoToMainMenu;

        private void OnEnable()
        {
            //_btnPopupA?.onClick.AddListener(OnPopupAButtonClicked);
            //_btnPopupB?.onClick.AddListener(OnPopupBButtonClicked);
            _btnGoToMainMenu?.onClick.AddListener(OnGoToMainMenuButtonClicked);
        }

        private void OnDisable()
        {
            //_btnPopupA?.onClick.RemoveListener(OnPopupAButtonClicked);
            //_btnPopupB?.onClick.RemoveListener(OnPopupBButtonClicked);
            _btnGoToMainMenu?.onClick.RemoveListener(OnGoToMainMenuButtonClicked);
        }

        //private void OnPopupAButtonClicked()
        //{
        //    ViewModel.RequestOpenPopupA();
        //}

        //private void OnPopupBButtonClicked()
        //{
        //    ViewModel.RequestOpenPopupB();
        //}

        private void OnGoToMainMenuButtonClicked()
        {
            ViewModel.RequestGoToMainMenu();
        }
    }
}
