using UnityEngine;
using UnityEngine.UI;

namespace MVVM.UI
{
    public abstract class PopupBinder<T> : WindowBinder<T> where T : WindowViewModel
    {
        [SerializeField] private Button _btnClose;
        [SerializeField] private Button _btnCloseAlt;

        protected void Start()
        {
            _btnClose?.onClick.AddListener(OnCloseButtonClick);
            _btnCloseAlt?.onClick.AddListener(OnCloseButtonClick);
        }

        protected void OnDestroy()
        {
            _btnClose?.onClick.RemoveListener(OnCloseButtonClick);
            _btnCloseAlt?.onClick.RemoveListener(OnCloseButtonClick);
        }

        protected virtual void OnCloseButtonClick()
        {
            ViewModel.RequestClose();
        }
    }
}
