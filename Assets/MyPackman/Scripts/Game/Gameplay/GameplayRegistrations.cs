using DI;
using Game.Services;

namespace Game.Gameplay
{
    public static class GameplayRegistrations   // Похож на MainMenuRegistrations
    {
        public static void Register(DIContainer container, GameplayEnterParams gameplayEnterParams)
        {
            // Регистрируем сервис сцены Gameplay
            container.RegisterFactory(c => new SomeGameplayService(c.Resolve<SomeCommonService>()));
        }
    }
}
