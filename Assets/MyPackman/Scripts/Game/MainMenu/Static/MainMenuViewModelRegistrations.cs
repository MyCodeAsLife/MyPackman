using DI;
using Game.UI;

namespace Game.MainMenu.Static
{
    // Для регистрации ViewModel-ей уровня сцены, а именно MainMenu
    // Зачем выносить его в статик? Можно его создавать в EntryPoint
    public static class MainMenuViewModelRegistrations  // Похож на GameplayViewModelRegistrations
    {
        public static void Register(DIContainer container)
        {
            // Регистрируем сервис сцены MainMenu как single
            container.RegisterFactory(c => new UIMainMenuRootViewModel()).AsSingle();
        }
    }
}
