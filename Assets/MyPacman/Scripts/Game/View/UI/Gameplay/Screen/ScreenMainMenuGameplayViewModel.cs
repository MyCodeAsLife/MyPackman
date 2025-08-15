using R3;

namespace MyPacman
{
    public class ScreenMainMenuGameplayViewModel : WindowViewModel
    {
        private readonly GameplayUIManager _uiManager;
        private readonly Subject<Unit> _exitSceneRequest;

        public ScreenMainMenuGameplayViewModel(GameplayUIManager uiManager, Subject<Unit> exitSceneRequest)
        {
            _uiManager = uiManager;
            _exitSceneRequest = exitSceneRequest;
        }

        public override string Id => "ScreenMainMenuGameplay";                          // Magic

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
