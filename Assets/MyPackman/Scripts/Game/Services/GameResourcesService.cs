using Game.Gameplay.Commands;
using Game.Gameplay.View;
using Game.State.cmd;
using Game.State.GameResources;
using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;

namespace Game.Services
{
    public class GameResourcesService
    {
        public readonly ObservableList<GameResourceViewModel> GameResources = new();

        private readonly Dictionary<GameResourceType, GameResourceViewModel> _resourcesMap = new();
        private readonly ICommanProcessor _cmd;

        public GameResourcesService(ObservableList<GameResource> resource, ICommanProcessor cmd)
        {
            _cmd = cmd;
            resource.ForEach(CreateGameResourceViewModel);
            resource.ObserveAdd().Subscribe(e => CreateGameResourceViewModel(e.Value));
            resource.ObserveRemove().Subscribe(e => RemoveGameResourceViewModel(e.Value));
        }

        public bool AddGameResource(GameResourceType gameResourceType, int amount)
        {
            var command = new CmdGameResourcesAdd(gameResourceType, amount);

            return _cmd.Procces(command);
        }

        public bool TrySpendGameResource(GameResourceType gameResourceType, int amount)
        {
            var command = new CmdGameResourcesSpend(gameResourceType, amount);

            return _cmd.Procces(command);
        }

        public bool IsEnouthGameResorces(GameResourceType gameResourceType, int amount)
        {
            if (_resourcesMap.TryGetValue(gameResourceType, out var resorces))
                return resorces.Amount.CurrentValue >= amount;

            return false;
        }

        public Observable<int> ObserveResource(GameResourceType gameResourceType)
        {
            if (_resourcesMap.TryGetValue(gameResourceType, out var resorces))
                return resorces.Amount;

            throw new Exception($"Resource of type {gameResourceType} doesn't exist");
        }

        private void CreateGameResourceViewModel(GameResource resource)
        {
            var resourceViewModel = new GameResourceViewModel(resource);
            _resourcesMap[resource.GameResourceType] = resourceViewModel;

            GameResources.Add(resourceViewModel);
        }

        private void RemoveGameResourceViewModel(GameResource resource)
        {
            if (_resourcesMap.TryGetValue(resource.GameResourceType, out var resourceViewModel))
            {
                GameResources.Remove(resourceViewModel);
                _resourcesMap.Remove(resource.GameResourceType);
            }
        }
    }
}
