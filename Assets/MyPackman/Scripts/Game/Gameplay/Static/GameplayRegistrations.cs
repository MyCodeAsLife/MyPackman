﻿using DI;
using Game.Gameplay.Commands;
using Game.Services;
using Game.Settings;
using Game.State;
using Game.State.cmd;
using Game.State.Maps;
using System;
using System.Linq;

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
            var settingsProvader = container.Resolve<ISettingsProvider>();
            var gameSettings = settingsProvader.GameSettings;

            // Создаем процессор команд а также обработчик строений
            var cmd = new CommandProcessor(gameStateProvider);
            // Регистрируем обработчик строений в процессоре команд
            cmd.RegisterHandler(new CmdPlaseBuldingHandler(gameState));
            // Регистрируем обработчик карт в процессоре команд
            cmd.RegisterHandler(new CmdCreateMapStateHandler(gameState, gameSettings));
            // Регистрируем процессор в DI контейнере сцены
            container.RegisterInstance<ICommanProcessor>(cmd);


            // На данный момент мы знаем, что мы пытаемся загрузить карту. Но не знаем, есть ли ее состояние вообще.
            // Создание карты - это модель, так что работать с ней нужно через команды, поэтому нужен обработчик команд
            // на случай, если состояния карты еще не суествует. Может мы этот момент передалаем потом, чтобы 
            // состояние карты создавалось ДО загрузки сцены и тут не было подобных проверок, но пока так. Делаем пошагово
            int loadingMapId = gameplayEnterParams.MapId;
            Map loadingMap = gameState.Maps.FirstOrDefault(m => m.Id == loadingMapId);

            // Создание состояния, если его еще нет
            if (loadingMap == null)
            {
                var command = new CmdCreateMapState(loadingMapId);
                bool success = cmd.Procces(command);

                if (success == false)
                {
                    throw new Exception($"Couldn't create map state with id: ${loadingMapId}");
                }

                // Это финальная проверка того что карта создалась успешно, иначе исключение
                loadingMap = gameState.Maps.First(map => map.Id == loadingMapId);
            }

            // Регистрируем создание сервиса строительства/перемещения/удаления зданий
            container.RegisterFactory(_ => new BuildingsService(
                loadingMap.Buildings,
                gameSettings.BuildingsSettings,
                cmd)
            ).AsSingle(); // Сервис должен быть в единственном экземпляре
        }
    }
}
