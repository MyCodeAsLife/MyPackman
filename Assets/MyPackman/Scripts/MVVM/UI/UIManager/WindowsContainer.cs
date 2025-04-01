using System.Collections.Generic;
using UnityEngine;

namespace MVVM.UI.UIManager
{
    public class WindowsContainer : MonoBehaviour
    {
        [SerializeField] private Transform _screensContainer;
        [SerializeField] private Transform _popupsContainer;

        private readonly Dictionary<WindowViewModel, IWindowBinder> _openedPopupBinders = new();
        private IWindowBinder _openedScreenBinder;

        public void OpenPopup(WindowViewModel viewModel)
        {
            var binder = CreateBinder(viewModel, _popupsContainer);
            _openedPopupBinders.Add(viewModel, binder);
        }

        public void ClosePopup(WindowViewModel popupViewModel)
        {
            var binder = _openedPopupBinders[popupViewModel];

            binder?.Close();
            _openedPopupBinders.Remove(popupViewModel);
        }

        public void OpenScreen(WindowViewModel viewModel)
        {
            if (viewModel == null)
                return;

            _openedScreenBinder?.Close();
            _openedScreenBinder = CreateBinder(viewModel, _screensContainer);
        }

        private IWindowBinder CreateBinder(WindowViewModel viewModel, Transform container)
        {
            var prefabPath = GetPrefabPath(viewModel);
            // Загружаем как GameObject потому как черз интерфейсы загружать нельзя
            var prefab = Resources.Load<GameObject>(prefabPath);
            var createdObject = Instantiate(prefab, container);
            var binder = createdObject.GetComponent<IWindowBinder>();

            binder.Bind(viewModel);
            return binder;
        }

        private string GetPrefabPath(WindowViewModel viewModel)
        {
            return $"Prefabs/UI/{viewModel.Id}";
        }
    }
}
