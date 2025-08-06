namespace MyPacman
{
    public class GameplayUIManager : UIManager
    {
        private UIGameplayRootViewModel _rootUI;
        //private readonly Subject<Unit> _exitSceneRequest;

        public GameplayUIManager(DIContainer container) : base(container)
        {
            //_exitSceneRequest = container.Resolve<Subject<Unit>>(GameConstants.ExitSceneRequestTag);
        }

        private UIGameplayRootViewModel RootUI      // Ститль именования?
        {
            get
            {
                if (_rootUI == null)
                    _rootUI = SceneContainer.Resolve<UIGameplayRootViewModel>();

                return _rootUI;
            }
        }

        //public ScreenGameplayViewModel OpenScreenGameplay()
        //{
        //    var viewModel = new ScreenGameplayViewModel(this, _exitSceneRequest);
        //    var rootUI = Container.Resolve<UIGameplayRootViewModel>();  // Вызов тут, потому как в конструкторе еще неизвестно, создан или нет UIGameplayRootViewModel

        //    rootUI.OpenScreen(viewModel);

        //    return viewModel;
        //}

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

        public ScorePopupTextViewModel OpenScorePopup()
        {
            var scorePopup = new ScorePopupTextViewModel();
            //var rootUI = SceneContainer.Resolve<UIGameplayRootViewModel>();
            RootUI.OpenPopupText(scorePopup);
            return scorePopup;
        }

        public void CloseScorePopup(PopupTextViewModel viewModel)
        {
            RootUI.ClosePopupText(viewModel);
        }
    }
}
