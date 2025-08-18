using R3;

namespace MyPacman
{
    public class ScreenPauseMenuViewModel : WindowViewModel
    {
        private readonly GameplayUIManager _uiManager;
        private readonly Subject<Unit> _exitSceneRequest;

        public ScreenPauseMenuViewModel(GameplayUIManager uiManager, Subject<Unit> exitSceneRequest)
        {
            _uiManager = uiManager;
            _exitSceneRequest = exitSceneRequest;
        }

        public override string Id => "ScreenPauseMenu";                          // Magic

        // Здесь добавить методы перенаправляющие взаимодействия с элементами UI(кнопки, ползуки, тригеры и т.д.)

        //public void RequestOpenPopupA()
        //{
        //    _uiManager.OpenPopupA();
        //}

        //public void RequestOpenPopupB()
        //{
        //    _uiManager.OpenPopupB();
        //}

        public void RequestGoToMainMenu()
        {
            _exitSceneRequest.OnNext(Unit.Default);
        }
    }
}
