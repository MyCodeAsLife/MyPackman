using R3;
using ObservableCollections;

namespace MyPacman
{
    public class UIGameplayRootBinder : UIRootBinder
    {
        // Если захотим что-то свое, то оверрайдим OnBind
        protected override void OnBind(UIRootViewModel rootViewModel)
        {
            var viewModel = rootViewModel as UIGameplayRootViewModel;

            // Создаем View для уже существующих/открытых PopupTexts
            foreach (var popupText in viewModel.OpenedPopupTexts)
            {
                Subscriptions.Add(popupText);
            }

            // Подписываемся на создание новых всплывающих сообщений
            Subscriptions.Add(viewModel.OpenedPopupTexts.ObserveAdd().Subscribe(collectionAddEvent =>
            {
                UIContainer.OpenPopupText(collectionAddEvent.Value);
            }));

            // Подписываемся на закрытие уже открытых сообщений
            Subscriptions.Add(viewModel.OpenedPopupTexts.ObserveRemove().Subscribe(collectionRemoveEvent =>
            {
                UIContainer.ClosePopupText(collectionRemoveEvent.Value);
            }));
        }
    }
}