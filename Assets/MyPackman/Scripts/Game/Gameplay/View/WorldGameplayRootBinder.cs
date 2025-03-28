using ObservableCollections;
using R3;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.View
{
    public class WorldGameplayRootBinder : MonoBehaviour
    {
        private readonly Dictionary<int, BuildingBinder> _createBuildingsMap = new();
        private readonly CompositeDisposable _disposables = new();

        private WorldGameplayRootViewModel _viewModel;  // For tests

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        public void Bind(WorldGameplayRootViewModel viewModel)
        {
            _viewModel = viewModel;

            foreach (var buildingViewModel in viewModel.AllBuildings)
            {
                CreateBuilding(buildingViewModel);
            }

            // Подписка на: при добавление новой ViewModel, создавать для нее View
            _disposables.Add(viewModel.AllBuildings.ObserveAdd().Subscribe(e => { CreateBuilding(e.Value); }));
            // Подписка на: при удалении ViewModel, удалить относящийся к ней View
            _disposables.Add(viewModel.AllBuildings.ObserveRemove().Subscribe(e => { DestroyBuilding(e.Value); }));
        }

        // Конструктор строений на карте
        private void CreateBuilding(BuildingViewModel buildingViewModel)
        {
            var buildingLevel = buildingViewModel.Level.CurrentValue;
            var buildingType = buildingViewModel.TypeId;
            var prefabBuildingLevelPath = $"Prefabs/Gameplay/Buildings/Building_{buildingType}_{buildingLevel}";
            var buildingPrefab = Resources.Load<BuildingBinder>(prefabBuildingLevelPath);
            var createdBuilding = Instantiate(buildingPrefab);
            createdBuilding.Bind(buildingViewModel);
            _createBuildingsMap[buildingViewModel.BuildingEntityId] = createdBuilding;
        }

        private void DestroyBuilding(BuildingViewModel buildingViewModel)
        {
            if (_createBuildingsMap.TryGetValue(buildingViewModel.BuildingEntityId, out var buildingBinder))
            {
                Destroy(buildingBinder.gameObject);
                _createBuildingsMap.Remove(buildingViewModel.BuildingEntityId);
            }
        }

        // For tests
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _viewModel.HandleTestInput();
            }
        }
    }
}
