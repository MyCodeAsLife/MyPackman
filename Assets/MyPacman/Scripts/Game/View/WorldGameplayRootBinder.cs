using ObservableCollections;
using R3;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Класс в котором создаются View и объеденяются с ViewModel
    // Хранит в себе список префабов
    public class WorldGameplayRootBinder : MonoBehaviour
    {
        private readonly Dictionary<int, EntityBinder> _viewEntitiesMap = new();

        // Это на случай когда во время выгрузки сцены, данный объект удалится раньше ViewModel-ей
        // И они при своем удалении будут пытатся обрашатся к данному объекту на удаление View
        private readonly CompositeDisposable _disposables = new();

        //private WorldGameplayRootViewModel _viewModel;

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        public void Bind(WorldGameplayRootViewModel rootViewModel)
        {
            //_viewModel = rootViewModel; // For Tests

            foreach (var viewModel in rootViewModel.AllEntities)
            {
                CreateEntityView(viewModel);
            }

            // Подписываем создание View, на появление новых ViewModel
            _disposables.Add(rootViewModel.AllEntities.ObserveAdd().Subscribe(/*(System.Action<CollectionAddEvent<EntityViewModel>>)*/(e =>
            {
                CreateEntityView(e.Value);
            })));
            // Подписываем удаление View, на удаление ViewModel
            _disposables.Add(rootViewModel.AllEntities.ObserveRemove().Subscribe(/*(System.Action<CollectionRemoveEvent<EntityViewModel>>)*/(e =>
            {
                DestroyEntityView(e.Value);
            })));
        }

        private void CreateEntityView(EntityViewModel entityViewModel)
        {
            string prefabBuildingPath = entityViewModel.PrefabPath;
            var prefabBuilding = Resources.Load<EntityBinder>(prefabBuildingPath);
            var createdBuilding = Instantiate(prefabBuilding);     // Создаем View объекта
            createdBuilding.Bind(entityViewModel);                // Объеденяем его с ViewModel

            // По хорошему, создаваемые View нужно кэшировать, чтобы проще было их удалять
            // Или переложить ответственность на их удаление на сам объект (тоесть подписать функцию удаление на евент удаления)
            _viewEntitiesMap[entityViewModel.EntityId] = createdBuilding;
        }

        private void DestroyEntityView(EntityViewModel entityViewModel)
        {
            if (_viewEntitiesMap.TryGetValue(entityViewModel.EntityId, out var createdBuilding))
            {
                Destroy(createdBuilding.gameObject);
                _viewEntitiesMap.Remove(entityViewModel.EntityId);
            }
        }
    }
}