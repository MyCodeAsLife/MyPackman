using DI;
using Game.Services;
using Game.State;

namespace Game.Gameplay.Static
{
    // Здесь регистрируются сервисы уровня сцены, а именно сцены Gameplay
    // Зачем выносить его в статик? Можно его создавать в EntryPoint
    public static class GameplayRegistrations   // Похож на MainMenuRegistrations
    {
        public static void Register(DIContainer container, GameplayEnterParams gameplayEnterParams)
        {
           // Регистрируем лямбду которая будет создавать сервис сцены и подгружать в него:
           // Сотояние игры(созданное/загруженное) в сцене MainMenu
           // И Некий сервис уровня проекта, который тутже создается если он не был создан ранее
            container.RegisterFactory(c => new SomeGameplayService(
                c.Resolve<IGameStateProvider>().GameState,
                c.Resolve<SomeCommonService>())
            ).AsSingle(); // Регистрируем сервис сцены Gameplay как single
        }
    }
}
