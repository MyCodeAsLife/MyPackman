using Game.Settings;
using Game.State.cmd;
using Game.State.Entities;
using Game.State.Entities.Mergeable.Buildings;
using Game.State.Maps;
using Game.State.Root;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Gameplay.Commands
{
    public class CmdCreateMapHandler : ICommandHandler<CmdCreateMap>
    {
        private readonly GameStateProxy _gameState;
        private readonly GameSettings _gameSettings;

        public CmdCreateMapHandler(GameStateProxy gameState, GameSettings gameSettings)
        {
            _gameState = gameState;
            _gameSettings = gameSettings;
        }

        public bool Handle(CmdCreateMap command)
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
            var initialEntities = new List<EntityData>();

            // Достаем дефолтные настройки строений и из них создаем состояния(строения)
            foreach (var buildingSettings in newMapInitialStateSettings.InitialBuildings)
            {
                var initialBuilding = new BuildingEntityData()
                {
                    UniqueId = _gameState.CreateEntityId(),
                    ConfigId = buildingSettings.ConfigId,
                    Type = EntityType.Building,
                    Position = buildingSettings.Position,
                    Level = buildingSettings.Level,
                    IsAutoCollectionEnabled = false,
                    LastClickedTimeMS = 0,
                };

                initialEntities.Add(initialBuilding);
            }

            // Создаем карту из того состояние что получили выше
            var newMapData = new MapData()
            {
                Id = command.MapId,
                Entities = initialEntities,
            };

            // Создаем прокси для новой карты
            var newMap = new Map(newMapData);
            // Добавляем в список карт(кэшируем)
            _gameState.Maps.Add(newMap);
            return true;
        }
    }
}
