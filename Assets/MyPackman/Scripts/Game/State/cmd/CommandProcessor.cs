using System;
using System.Collections.Generic;

namespace Game.State.cmd
{
    public class CommandProcessor : ICommanProcessor
    {
        // Словарь непозволяет хранить разные объекты в словаре, поэтому используем object, все добавляемые объекты приводятся к нему
        private readonly Dictionary<Type, object> _handlesMap = new();
        private readonly IGameStateProvider _gameStateProvider;

        public CommandProcessor(IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
        }

        public void RegisterHandler<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand
        {
            _handlesMap[typeof(TCommand)] = handler;
        }

        public bool Procces<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (_handlesMap.TryGetValue(typeof(TCommand), out var handler))
            {
                // Конвертируем из object обратно в ICommandHandler<TCommand>
                var typedHandler = (ICommandHandler<TCommand>)handler;
                // Обрабатываем\выполняем команду
                var result = typedHandler.Handle(command);
                // Если команда выполнилась, то состояние игры сохраняется
                if (result)
                {
                    _gameStateProvider.SaveGameState();
                }

                return result;
            }

            // Если обработать неполучилось
            return false;
        }
    }
}
