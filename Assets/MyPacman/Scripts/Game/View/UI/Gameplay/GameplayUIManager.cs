namespace MyPacman
{
    public class GameplayUIManager : UIManager  // Через интерфейсы разделить доступные методы для разных сервисов
    {
        private UIGameplayRootViewModel _rootUI;
        //private readonly Subject<Unit> _exitSceneRequest;

        public GameplayUIManager(DIContainer container) : base(container)
        {
            //_exitSceneRequest = container.Resolve<Subject<Unit>>(GameConstants.ExitSceneRequestTag);
            _rootUI = SceneContainer.Resolve<UIGameplayRootViewModel>();
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
