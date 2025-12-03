using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    public class GhostsStateHandler
    {
        public readonly Dictionary<EntityType, Ghost> GhostsMap = new();

        private readonly GhostMovementServicesHandler _ghostMovementServicesHandler;
        private readonly BehaviourModesFactory _behaviourModesFactory;
        private readonly Pacman _pacman;

        public GhostBehaviorModeType GlobalStateOfGhosts;     // Это лишнее?

        public GhostsStateHandler(
            IObservableCollection<Entity> entities,
            Pacman pacman,
            ReactiveProperty<int> pacmanLifePoints,
            Func<SpawnPointType, Vector2> getSpawnPosition,
            TimeService timeService,
            HandlerOfPickedEntities mapHandlerService,
            ILevelConfig levelConfig)
        {
            var blinkyPosition = entities.First(e => e.Type == EntityType.Blinky).Position;
            Vector2 mapSize = new Vector2(levelConfig.Map.GetLength(1), -levelConfig.Map.GetLength(0));
            _pacman = pacman;

            _behaviourModesFactory = new BehaviourModesFactory(
                mapHandlerService,
                blinkyPosition,
                pacman.Position,
                pacman.Direction,
                mapSize,
                getSpawnPosition);

            _ghostMovementServicesHandler = new GhostMovementServicesHandler(
                timeService,
                entities,
                pacman.Position,
                levelConfig);

            InitGhostsMap(entities);
        }

        public Dictionary<EntityType, GhostMovementService> GhostMovementServicesMap
            => _ghostMovementServicesHandler.GhostMovementServicesMap;

        public float GetTimerForBehaviorType(GhostBehaviorModeType behaviorModeType)    // global ghost state handler
        {
            switch (behaviorModeType)
            {
                case GhostBehaviorModeType.Scatter:
                    return GameConstants.ScatterTimer;

                case GhostBehaviorModeType.Chase:
                    return GameConstants.ChaseTimer;

                default:
                    throw new Exception(GameConstants.NoSwitchingDefined);
            }
        }

        public void SetBehaviourModeEveryone(GhostBehaviorModeType behaviorModeType)    // global ghost state handler
        {
            GlobalStateOfGhosts = behaviorModeType;

            foreach (var ghost in GhostsMap)
                SetBehaviourMode(ghost.Key, behaviorModeType);
        }

        public void SetBehaviourMode(EntityType entityType, GhostBehaviorModeType behaviorModeType)
        {
            var movementService = GhostMovementServicesMap[entityType];
            var entity = GhostsMap[entityType];
            var behaviourMode = _behaviourModesFactory.CreateMode(behaviorModeType, entity);
            movementService.BindBehaviorMode(behaviourMode);
        }

        public void ShowGhosts()       // global ghost state handler
        {
            foreach (var shost in GhostsMap)
                shost.Value.ShowGhost();
        }

        public void HideGhosts()        // global ghost state handler
        {
            foreach (var ghost in GhostsMap)
                ghost.Value.HideGhost();
        }

        public bool IsPacmanReached(EntityType ghostType)   // PacmanStateHandler ?
        {
            return GhostsMap[ghostType].Position.CurrentValue.SqrDistance(_pacman.Position.CurrentValue) < 1.2f;// Magic (расстояние между призраком и игроком)
        }

        private void InitGhostsMap(IObservableCollection<Entity> entities)
        {
            foreach (var entity in entities)
                if (IsGhostEntity(entity.Type))
                    GhostsMap.Add(entity.Type, entity as Ghost);

            entities.ObserveAdd().Subscribe(e =>
            {
                if (IsGhostEntity(e.Value.Type))
                    GhostsMap.Add(e.Value.Type, e.Value as Ghost);
            });

            entities.ObserveRemove().Subscribe(e => GhostsMap.First(value => value.Key == e.Value.Type));
        }

        private bool IsGhostEntity(EntityType entity) => entity <= EntityType.Blinky && entity >= EntityType.Clyde;
    }
}