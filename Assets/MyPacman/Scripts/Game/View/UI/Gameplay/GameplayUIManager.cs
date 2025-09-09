using R3;

namespace MyPacman
{
    public class GameplayUIManager : UIManager  // Через интерфейсы разделить доступные методы для разных сервисов
    {
        private readonly UIGameplayRootViewModel _rootUI;
        private readonly Subject<Unit> _exitSceneRequest;

        public GameplayUIManager(DIContainer viewModelContainer) : base(viewModelContainer)
        {
            _exitSceneRequest = viewModelContainer.Resolve<Subject<Unit>>(GameConstants.SceneExitRequestTag);
            _rootUI = Container.Resolve<UIGameplayRootViewModel>();
        }

        public bool IsScreenOpen => _rootUI.OpenedScreen.CurrentValue != null;
        //public ReadOnlyReactiveProperty<WindowViewModel> OpenedScreen => _rootUI.OpenedScreen;

        public void OpenUIGameplay()
        {
            var viewModel = Container.Resolve<IUIGameplayViewModel>() as UIGameplayViewModel;
            _rootUI.CreateGameplayUI(viewModel);
        }

        public ScreenPauseMenuViewModel OpenScreenPauseMenu()
        {
            var viewModel = new ScreenPauseMenuViewModel(this, _exitSceneRequest);
            _rootUI.OpenScreen(viewModel);
            return viewModel;
        }

        public void CloseScreenPauseMenu()
        {
            _rootUI.CloseScreen();
        }

        //public PopupAViewModel OpenPopupA()     // Похож на OpenPopupB
        //{
        //    var popupA = new PopupAViewModel();
        //    var rootUI = SceneContainer.Resolve<UIGameplayRootViewModel>();  // Вызов тут, потому как в конструкторе еще неизвестно, создан или нет UIGameplayRootViewModel

        //    rootUI.OpenPopup(popupA);

        //    return popupA;
        //}

        //public PopupBViewModel OpenPopupB()     // Похож на OpenPopupA
        //{
        //    var popupB = new PopupBViewModel();
        //    var rootUI = SceneContainer.Resolve<UIGameplayRootViewModel>();  // Вызов тут, потому как в конструкторе еще неизвестно, создан или нет UIGameplayRootViewModel

        //    rootUI.OpenPopup(popupB);

        //    return popupB;
        //}

        public ScorePopupTextViewModel OpenScorePopupText()
        {
            var scorePopup = new ScorePopupTextViewModel();
            _rootUI.OpenPopupText(scorePopup);
            return scorePopup;
        }

        public void CloseScorePopupText(PopupTextViewModel viewModel)
        {
            _rootUI.ClosePopupText(viewModel);
        }
    }
}
