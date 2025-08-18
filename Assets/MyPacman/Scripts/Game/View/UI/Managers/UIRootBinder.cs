using R3;
using UnityEngine;

namespace MyPacman
{
    // Отвечает за все подписки Вьюх на Вьюмодели
    public class UIRootBinder : MonoBehaviour
    {
        [SerializeField] protected UIContainer UIContainer;

        protected readonly CompositeDisposable Subscriptions = new();    // Для отписок

        public void Bind(UIRootViewModel viewModel)
        {
            // Подписываемся на изменение viewModel.OpenedScreen, тоесть когда пришел на запрос открытия нового окна
            Subscriptions.Add(viewModel.OpenedScreen.Subscribe(newScreenViewModel =>
            {
                UIContainer.OpenScreen(newScreenViewModel);         // Проверить не вызывается ли это при закрытии окна
            }));

            //// Создаем View для уже существующих/открытых Popups
            //foreach (var popup in viewModel.OpenedPopups)
            //{
            //    _subscriptions.Add(popup);
            //}

            //// Пописываемся на открытие новых Popups
            //_subscriptions.Add(viewModel.OpenedPopups.ObserveAdd().Subscribe(collectionAddEvent =>
            //{
            //    _UIContainer.OpenPopup(collectionAddEvent.Value);
            //}));

            //// Подписываемся на закрытие уже открытых Popups
            //_subscriptions.Add(viewModel.OpenedPopups.ObserveRemove().Subscribe(collectionRemoveEvent =>
            //{
            //    _UIContainer.ClosePopup(collectionRemoveEvent.Value);
            //}));

            OnBind(viewModel);
        }

        protected virtual void OnBind(UIRootViewModel viewModel) { }

        private void OnDestroy()
        {
            foreach (var uiElement in Subscriptions)
                uiElement?.Dispose();
        }
    }
}
