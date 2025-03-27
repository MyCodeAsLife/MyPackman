using Game.Settings;
using Game.State.cmd;
using Game.State.Entities.Buildings;
using Game.State.Maps;
using Game.State.Root;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Gameplay.Commands
{
    public class CmdCreateMapStateHandler : ICommandHandler<CmdCreateMapState>
    {
        private readonly GameStateProxy _gameState;
        private readonly GameSettings _gameSettings;

        public CmdCreateMapStateHandler(GameStateProxy gameState, GameSettings gameSettings)
        {
            _gameState = gameState;
            _gameSettings = gameSettings;
        }

        public bool Handle(CmdCreateMapState command)
        {
            var isMapAlreadyExisted = _gameState.Maps.Any(m => m.Id == command.MapId);

            // Проверяем загружена ли уже данная карта
            if (isMapAlreadyExisted)
            {
                Debug.LogError($"Map with Id = {command.MapId} alredy exists");
                return false;
            }

            // Вытаскиваем настройки карты, если неполучится то выпадет исключение что значит либо команда неверная, либо чтото с картой
            var newMapSettings = _gameSettings.MapsSettings.Maps.First(m => m.MapId == command.MapId);
            // Вытаскиваем дефолные настройки карты (зачем они расщелены на 2 типа? MapSettings и MapInitialStateSettings)
            var newMapInitialStateSettings = newMapSettings.InitialStateSettings;
            var initialBuildings = new List<BuildingEntity>();

            // Достаем дефолтные настройки строений и из них создаем состояния(строения)
            foreach (var buildingSettings in newMapInitialStateSettings.InitialBuildings)
            {
                var initialBuilding = new BuildingEntity
                {
                    Id = _gameState.CreateEntityId(),
                    TypeId = buildingSettings.TypeId,
                    Position = buildingSettings.Position,
                    Level = buildingSettings.Level,
                };

                initialBuildings.Add(initialBuilding);
            }

            // Создаем карту из того состояние что получили выше
            var newMapState = new MapState()
            {
                Id = command.MapId,
                Buildings = initialBuildings,
            };

            // Создаем прокси для новой карты
            var newMap = new Map(newMapState);
            // Добавляем в список карт(кэшируем)
            _gameState.Maps.Add(newMap);
            return true;
        }
    }
}
