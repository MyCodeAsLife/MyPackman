using DI;
using Game.Gameplay.View;
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
            container.RegisterFactory(c => new GameplayUIManager(container)).AsSingle();
            container.RegisterFactory(c => new UIGameplayRootViewModel()).AsSingle();
            container.RegisterFactory(c => new WorldGameplayRootViewModel(
                //c.Resolve<BuildingsService>(),
                c.Resolve<GameResourcesService>())
            ).AsSingle();
        }
    }
}
