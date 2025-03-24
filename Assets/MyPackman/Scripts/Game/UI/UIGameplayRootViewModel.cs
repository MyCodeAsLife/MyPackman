using Game.Services;

namespace Game.UI
{
    public class UIGameplayRootViewModel
    {
        private SomeGameplayService _gameplayService;

        public UIGameplayRootViewModel(SomeGameplayService gameplayService)
        {
            _gameplayService = gameplayService;
        }
    }
}
