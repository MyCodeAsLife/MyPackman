using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Создает, хранит и закрывает View
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private Transform _screensContainer;
        //[SerializeField] private Transform _popupsContainer;
        [SerializeField] private Transform _popupTextsContainer;

        //private readonly Dictionary<WindowViewModel, IWindowBinder> _openedPopupBinders = new();
        private readonly Dictionary<PopupTextViewModel, IUIBinder> _openedPopupTextBinders = new();
        private IUIBinder _openedScreenBinder;

        public void OpenPopupText(PopupTextViewModel viewModel)
        {
            IUIBinder binder = CreatePopupTextView(viewModel, _popupTextsContainer);
            _openedPopupTextBinders.Add(viewModel, binder);
        }

        public void ClosePopupText(PopupTextViewModel popupViewModel)
        {
            var binder = _openedPopupTextBinders[popupViewModel];
            binder?.Close();
            _openedPopupTextBinders.Remove(popupViewModel);
        }

        //public void OpenPopup(WindowViewModel viewModel)
        //{
        //    IWindowBinder binder = CreateView(viewModel, _popupsContainer);
        //    _openedPopupBinders.Add(viewModel, binder);
        //}

        //public void ClosePopup(WindowViewModel popupViewModel)
        //{
        //    var binder = _openedPopupBinders[popupViewModel];
        //    binder?.Close();
        //    _openedPopupBinders.Remove(popupViewModel);
        //}

        public void OpenScreen(WindowViewModel viewModel)
        {
            if (viewModel == null)
                return;

            IUIBinder binder = CreateWindowView(viewModel, _screensContainer);
            _openedScreenBinder = binder;
        }

        private string GetPrefabPath(string id)
        {
            return GameConstants.UIFolderPath + id;
        }

        private IUIBinder CreateWindowView(WindowViewModel viewModel, Transform container)
        {
            var prefabPath = GetPrefabPath(viewModel.Id);
            var prefab = Resources.Load<GameObject>(prefabPath);
            var createdPopup = Instantiate(prefab, container);
            var binder = createdPopup.GetComponent<IUIBinder>();
            binder.Bind(viewModel);

            return binder;
        }

        private IUIBinder CreatePopupTextView(PopupTextViewModel viewModel, Transform container)
        {
            var createdPopup = CreateView(viewModel.Id, container);
            var binder = createdPopup.GetComponent<IUIBinder>();
            binder.Bind(viewModel);
            return binder;
        }

        private GameObject CreateView(string id, Transform container)
        {
            string prefabPath = GetPrefabPath(id);
            var prefab = Resources.Load<GameObject>(prefabPath);
            return Instantiate(prefab, container);
        }
    }
}