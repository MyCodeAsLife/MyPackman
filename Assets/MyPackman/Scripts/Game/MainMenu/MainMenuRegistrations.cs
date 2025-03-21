using DI;
using Game.Services;

namespace Game.MainMenu
{
    public static class MainMenuRegistrations   // Похож на GameplayRegistrations
    {
        public static void Register(DIContainer container, MainMenuEnterParams mainMenuEnterParams)
        {
            // Регистрируем сервис сцены Mainmenu
            container.RegisterFactory(c => new SomeMainMenuService(c.Resolve<SomeCommonService>()));
        }
    }
}
