using DI;
using Game.Services;

namespace Game.MainMenu.Static
{
    // Здесь регистрируются сервисы уровня сцены, а именно сцены MainMenu
    // Зачем выносить его в статик? Можно его создавать в EntryPoint
    public static class MainMenuRegistrations   // Похож на GameplayRegistrations
    {
        public static void Register(DIContainer container, MainMenuEnterParams mainMenuEnterParams)
        {
            // Регистрируем сервис сцены MainMenu как single
            container.RegisterFactory(c => new SomeMainMenuService(c.Resolve<SomeCommonService>())).AsSingle();
        }
    }
}
