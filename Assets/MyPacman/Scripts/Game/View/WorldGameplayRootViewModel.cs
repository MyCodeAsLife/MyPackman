using ObservableCollections;
using R3;
using System.Collections.Generic;

namespace MyPacman
{
    public class WorldGameplayRootViewModel
    {
        private readonly ObservableList<EntityViewModel> _allEntities = new();
        private readonly Dictionary<int, EntityViewModel> _entitiesMap = new();  // Кэшируем созданные ViewModel, для быстрого доступа к последним в будущем

        public WorldGameplayRootViewModel(IObservableCollection<Entity> entities)
        {
            // Пробегаемся по состояниям и для каждого состояние создаем ViewModel
            foreach (var entity in entities)
            {
                CreateEntityViewModel(entity);
            }

            // Подписываем создание новых ViewModel для новодобовляющихся состояний
            entities.ObserveAdd().Subscribe(e =>
            {
                CreateEntityViewModel(e.Value);
            });

            // Подписываем удаление текущих ViewModel при удалении их состояний
            entities.ObserveRemove().Subscribe(e =>
            {
                RemoveBuildingViewModel(e.Value);
            });
        }

        public IObservableCollection<EntityViewModel> AllEntities => _allEntities;

        private void CreateEntityViewModel(Entity entity)     // Создание ViewModel'ей нужно прогонять через фабрику
        {
            var factory = new ViewModelFactory();
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
