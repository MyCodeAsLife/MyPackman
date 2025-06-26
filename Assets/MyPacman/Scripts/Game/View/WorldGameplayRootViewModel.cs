using ObservableCollections;
using R3;
using System.Collections.Generic;

namespace MyPacman
{
    public class WorldGameplayRootViewModel
    {
        //private readonly ResourcesService _resourcesService;

        // new
        private readonly ObservableList<EntityViewModel> _allEntities = new();
        private readonly Dictionary<int, EntityViewModel> _entitiesMap = new();  // Кэшируем созданные ViewModel, для быстрого доступа к последним в будущем
        //private readonly Dictionary<string, BuildingSettings> _buildingsSettingsMap = new();   // Кэшируем список настроек для всех типов строений

        public WorldGameplayRootViewModel(IObservableCollection<Entity> entities/*, EntitiesSettings buildingsSettings*/)
        {
            //AllEntities = buildingsService.AllBuildings;   // Кэшируем доступ к реактивному списку ViewModel's

            //// Формируем список настроек для всех типов строений
            //foreach (var buildingSettings in buildingsSettings.Buildings)
            //{
            //    _buildingsSettingsMap[buildingSettings.ConfigId] = buildingSettings;
            //}

            // Пробегаемся по состояниям и для каждого состояние создаем ViewModel
            foreach (var entity in entities)
            {
                //if (entity is BuildingEntity buildingEntity)
                CreateBuildingViewModel(entity);
            }

            // Подписываем создание новых ViewModel для новодобовляющихся состояний
            entities.ObserveAdd().Subscribe(e =>
            {
                //if (e.Value is BuildingEntity buildingEntity)
                CreateBuildingViewModel(e.Value);
            });

            // Подписываем удаление текущих ViewModel при удалении их состояний
            entities.ObserveRemove().Subscribe(e =>
            {
                //if (e.Value is BuildingEntity buildingEntity)
                RemoveBuildingViewModel(e.Value);
            });
        }

        public IObservableCollection<EntityViewModel> AllEntities => _allEntities;

        private void CreateBuildingViewModel(Entity entity)     // Создание ViewModel'ей нужно прогонять через фабрику
        {
            //var buildingSettings = _buildingsSettingsMap[entity.ConfigId];
            var factory = new ViewModelFactory();
            //var entityViewModel = new EntityViewModel(entity/*, buildingSettings, this*/);
            var entityViewModel = factory.CreateEntityViewModel(entity);
            _allEntities.Add(entityViewModel);
            _entitiesMap[entity.UniqueId] = entityViewModel;
        }

        private void RemoveBuildingViewModel(Entity entity)
        {
            if (_entitiesMap.TryGetValue(entity.UniqueId, out var buildingViewModel))
            {
                _entitiesMap.Remove(entity.UniqueId);
                _allEntities.Remove(buildingViewModel);
            }
        }
    }
}
