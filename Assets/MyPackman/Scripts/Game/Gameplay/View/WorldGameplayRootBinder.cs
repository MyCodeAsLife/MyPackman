using ObservableCollections;
using R3;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.View
{
    public class WorldGameplayRootBinder : MonoBehaviour
    {
        //[SerializeField] private BuildingBinder _prefabBuilding;

        private readonly Dictionary<int, BuildingBinder> _createBuildingsMap = new();

        private readonly CompositeDisposable _disposables = new();

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        public void Bind(WorldGameplayRootViewModel viewModel)
        {
            foreach (var buildingViewModel in viewModel.AllBuildings)
            {
                CreateBuilding(buildingViewModel);
            }

            // Подписка на: при добавление новой ViewModel, создавать для нее View
            _disposables.Add(viewModel.AllBuildings.ObserveAdd().Subscribe(e => { CreateBuilding(e.Value); }));
            // Подписка на: при удалении ViewModel, удалить относящийся к ней View
            _disposables.Add(viewModel.AllBuildings.ObserveRemove().Subscribe(e => { DestroyBuilding(e.Value); }));
        }

        private void CreateBuilding(BuildingViewModel buildingViewModel)
        {
            // заглушка
            var buildingLevel = Random.Range(1, 4);

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
    }
}
