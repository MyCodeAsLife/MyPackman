using ObservableCollections;
using R3;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    public class GhostMovementServicesHandler
    {
        public readonly Dictionary<EntityType, GhostMovementService> GhostMovementServicesMap = new();

        private readonly TimeService _timeService;

        public GhostMovementServicesHandler(
            TimeService timeService,
            IObservableCollection<Entity> entities,
            ReadOnlyReactiveProperty<Vector2> pacmanPosition,
            ILevelConfig levelConfig)
        {
            _timeService = timeService;

            InitGhostMovementServicesMap(entities, pacmanPosition, levelConfig);
        }

        private void InitGhostMovementServicesMap(
            IObservableCollection<Entity> entities,
            ReadOnlyReactiveProperty<Vector2> pacmanPosition,
            ILevelConfig levelConfig)
        {
            foreach (var entity in entities)
                TryCreateMovementService(entity, pacmanPosition, levelConfig);

            entities.ObserveAdd().Subscribe(e => TryCreateMovementService(e.Value, pacmanPosition, levelConfig));
            entities.ObserveRemove().Subscribe(e => TryDestroyMovementService(e.Value.Type));
        }

        public bool TryCreateMovementService(
            Entity entity,
            ReadOnlyReactiveProperty<Vector2> pacmanPosition,
            ILevelConfig levelConfig)
        {
            if (IsGhostEntity(entity.Type))
            {
                var ghostMovementService = new GhostMovementService(
                    entity as Ghost,
                    pacmanPosition,
                    _timeService,
                    levelConfig);
                GhostMovementServicesMap.Add(entity.Type, ghostMovementService);
                return true;
            }

            return false;
        }

        public bool TryDestroyMovementService(EntityType entityType)
        {
            if (GhostMovementServicesMap.ContainsKey(entityType))
            {
                GhostMovementServicesMap.Remove(entityType);
                return true;
            }

            return false;
        }

        private bool IsGhostEntity(EntityType entity) => entity <= EntityType.Blinky && entity >= EntityType.Clyde;
    }
}