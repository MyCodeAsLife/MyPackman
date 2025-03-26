using Game.Services;
using Game.Settings.Gameplay.Buildings;
using Game.State.Buildings;
using R3;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.View
{
    public class BuildingViewModel
    {
        private readonly BuildingEntityProxy _buildingEntity;
        private readonly BuildingSettings _buildingSettings;
        private readonly BuildingsService _buildingsService;
        private readonly Dictionary<int, BuildingLevelSettings> _levelSettingsMap = new(); // Кэширование для увеличения производительности

        public readonly int BuildingEntityId;
        public readonly string TypeId;

        public ReadOnlyReactiveProperty<Vector3Int> Position { get; }

        public BuildingViewModel(BuildingEntityProxy buildingEntity,
                                 BuildingSettings buildingSettings,
                                 BuildingsService buildingsService)
        {
            TypeId = buildingEntity.TypeId;
            _buildingEntity = buildingEntity;
            _buildingSettings = buildingSettings;
            _buildingsService = buildingsService;

            Position = buildingEntity.Position;
            BuildingEntityId = buildingEntity.Id;

            foreach (var buildingLevelSettings in _buildingSettings.LevelSettings)
            {
                _levelSettingsMap[buildingLevelSettings.Level] = buildingLevelSettings;
            }
        }

        public BuildingLevelSettings GetLevelSettings(int level)
        {
            return _levelSettingsMap[level];
        }
    }
}
