using ObservableCollections;
using R3;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.View
{
    public class WorldGameplayRootBinder : MonoBehaviour
    {
        [SerializeField] private BuildingBinder _prefabBuilding;

        private readonly Dictionary<int, BuildingBinder> _createBuildingsMap = new();

        private readonly CompositeDisposable _disposables = new();

        public void Bind(WorldGameplayRootViewModel viewModel)
        {
            foreach (var buildingViewModel in viewModel.AllBuildings)
            {
                CreateBuilding(buildingViewModel);
            }

            _disposables.Add(viewModel.AllBuildings.ObserveAdd().Subscribe(e =>
            {
                CreateBuilding(e.Value);
            }));
        }

        private void CreateBuilding(BuildingViewModel buildingViewModel)
        {
            var createdBuilding = Instantiate(_prefabBuilding);
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
