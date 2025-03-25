using DI;
using Game.Gameplay.Commands;
using Game.Services;
using Game.State;
using Game.State.cmd;

namespace Game.Gameplay.Static
{
    // Здесь регистрируются сервисы уровня сцены, а именно сцены Gameplay
    // Зачем выносить его в статик? Можно его создавать в EntryPoint
    public static class GameplayRegistrations   // Похож на MainMenuRegistrations
    {
        public static void Register(DIContainer container, GameplayEnterParams gameplayEnterParams)
        {
            var gameStateProvider = container.Resolve<IGameStateProvider>();
            var gameState = gameStateProvider.GameState;

            // Создаем процессор команд а также обработчик строений
            var cmd = new CommandProcessor(gameStateProvider);
            // Регистрируем обработчик строений в процессоре команд
            cmd.RegisterHandler(new CmdPlaseBuldingHandler(gameState));
            // Регистрируем процессор в DI контейнере сцены
            container.RegisterInstance<ICommanProcessor>(cmd);

            // Регистрируем создание сервиса
            container.RegisterFactory(_ => new BuildingsService(gameState.Buildings, cmd)).AsSingle(); // Сервис должен быть в единственном экземпляре
        }
    }
}
