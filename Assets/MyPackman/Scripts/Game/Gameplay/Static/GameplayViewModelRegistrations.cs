using DI;
using Game.Services;
using Game.UI;

namespace Game.Gameplay.Static
{
    // Для регистрации ViewModel-ей уровня сцены, а именно Gameplay
    // Зачем выносить его в статик? Можно его создавать в EntryPoint
    public static class GameplayViewModelRegistrations  // Похож на MainMenuViewModelRegistrations
    {
        public static void Register(DIContainer container)
        {
            // Регистрируем сервис сцены Gameplay как single
            container.RegisterFactory(c => new UIGameplayRootViewModel(c.Resolve<SomeGameplayService>())).AsSingle();
            container.RegisterFactory(c => new WorldGameplayViewModel()).AsSingle();
        }
    }
}
