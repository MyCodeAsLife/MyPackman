using Game.Settings.Gameplay.Buildings;
using R3;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.View
{
    public class BuildingViewModel
    {
        //private readonly BuildingEntityProxy _buildingEntity;
        private readonly BuildingSettings _buildingSettings;
        //private readonly BuildingsService _buildingsService;
        private readonly Dictionary<int, BuildingLevelSettings> _levelSettingsMap = new(); // Кэширование для увеличения производительности

        public readonly int BuildingEntityId;
        public readonly string TypeId;

        public ReadOnlyReactiveProperty<Vector3Int> Position { get; }
        public ReadOnlyReactiveProperty<int> Level { get; }

        //public BuildingViewModel(BuildingEntityProxy buildingEntity,
        //                         BuildingSettings buildingSettings,
        //                         BuildingsService buildingsService)
        //{
        //    TypeId = buildingEntity.TypeId;
        //    _buildingEntity = buildingEntity;
        //    _buildingSettings = buildingSettings;
        //    _buildingsService = buildingsService;

        //    Position = buildingEntity.Position;
        //    BuildingEntityId = buildingEntity.Id;
        //    Level = buildingEntity.Level;

        //    foreach (var buildingLevelSettings in _buildingSettings.LevelSettings)
        //    {
        //        _levelSettingsMap[buildingLevelSettings.Level] = buildingLevelSettings;
        //    }
        //}

        public BuildingLevelSettings GetLevelSettings(int level)
        {
            return _levelSettingsMap[level];
        }
    }
}
