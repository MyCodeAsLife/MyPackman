using Game.Services;
using Game.State.GameResources;
using ObservableCollections;
using R3;
using System;
using UnityEngine;

namespace Game.Gameplay.View
{
    public class WorldGameplayRootViewModel
    {
        public readonly IObservableCollection<BuildingViewModel> AllBuildings;
        private readonly GameResourcesService _gameResourcesService;

        public WorldGameplayRootViewModel(BuildingsService buildingsService, GameResourcesService gameResourcesService)
        {
            AllBuildings = buildingsService.AllBuildings;
            _gameResourcesService = gameResourcesService;

            // For tests
            gameResourcesService.ObserveResource(GameResourceType.SoftCurrency)
                .Subscribe(newVlaue => Debug.Log($"SoftCurrency: {newVlaue}"));

            gameResourcesService.ObserveResource(GameResourceType.HardCurrency)
                .Subscribe(newVlaue => Debug.Log($"HardCurrency: {newVlaue}"));
        }

        // For tests
        public void HandleTestInput()
        {
            var rResourceType = (GameResourceType)UnityEngine.Random
                .Range(0, Enum.GetNames(typeof(GameResourceType)).Length);
            var rValue = UnityEngine.Random.Range(1, 1000);
            var rOperation = UnityEngine.Random.Range(0, 2);

            if (rOperation == 0)
            {
                _gameResourcesService.AddGameResource(rResourceType, rValue);
                return;
            }

            _gameResourcesService.TrySpendGameResource(rResourceType, rValue);
        }
    }
}
