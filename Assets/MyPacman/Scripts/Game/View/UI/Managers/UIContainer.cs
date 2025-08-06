using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Создает, хранит и закрывает View
    public class UIContainer : MonoBehaviour
    {
        //[SerializeField] private Transform _screensContainer;
        //[SerializeField] private Transform _popupsContainer;
        [SerializeField] private Transform _popupTextsContainer;

        //private readonly Dictionary<WindowViewModel, IWindowBinder> _openedPopupBinders = new();
        private readonly Dictionary<PopupTextViewModel, IPopupTextBinder> _openedPopupTextBinders = new();
        //private IWindowBinder _openedScreenBinder;

        public void OpenPopupText(PopupTextViewModel viewModel)
        {
            IPopupTextBinder binder = CreatePopupTextView(viewModel, _popupTextsContainer);
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

        //public void OpenScreen(WindowViewModel viewModel)
        //{
        //    if (viewModel == null)
        //        return;

        //    IWindowBinder binder = CreateView(viewModel, _screensContainer);
        //    _openedScreenBinder = binder;
        //}

        //private string GetPrefabPath(WindowViewModel viewModel)
        //{
        //    return GameConstants.UIFolderPath + viewModel.Id;
        //}

        //private IWindowBinder CreateWindowView(WindowViewModel viewModel, Transform container)    // Где присваивать позицию созданному объекту?
        //{
        //    var prefabPath = GetPrefabPath(viewModel);
        //    var prefab = Resources.Load<GameObject>(prefabPath);
        //    var createdPopup = Instantiate(prefab, container);
        //    var binder = createdPopup.GetComponent<IWindowBinder>();
        //    binder.Bind(viewModel);

        //    return binder;
        //}

        private IPopupTextBinder CreatePopupTextView(PopupTextViewModel viewModel, Transform container)
        {
            var createdPopup = CreateView(viewModel.Id, container);
            var binder = createdPopup.GetComponent<IPopupTextBinder>();
            binder.Bind(viewModel);
            return binder;
        }

        private GameObject CreateView(string id, Transform container)
        {
            string prefabPath = GameConstants.UIFolderPath + id;
            var prefab = Resources.Load<GameObject>(prefabPath);
            return Instantiate(prefab, container);
        }
    }
}