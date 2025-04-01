using DI;
using Game.Common;
using Game.UI.PopupA;
using Game.UI.PopupB;
using Game.UI.ScreenGameplay;
using MVVM.UI.UIManager;
using R3;

namespace Game.UI
{
    public class GameplayUIManager : UIManager
    {
        private readonly Subject<Unit> _exitSceneRequest;
        public GameplayUIManager(DIContainer container) : base(container)
        {
            _exitSceneRequest = container.Resolve<Subject<Unit>>(Constants.EXIT_SCENE_REQUEST_TAG);
        }

        public ScreenGameplayViewModel OpenScreenGameplay()
        {
            var viewModel = new ScreenGameplayViewModel(this, _exitSceneRequest);
            var rootUI = Container.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenScreen(viewModel);
            return viewModel;
        }

        public PopupAViewModel OpenPopupA()
        {
            var popupA = new PopupAViewModel();
            var rootUI = Container.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenPopup(popupA);
            return popupA;
        }

        public PopupBViewModel OpenPopupB()
        {
            var popupB = new PopupBViewModel();
            var rootUI = Container.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenPopup(popupB);
            return popupB;
        }

    }
}
