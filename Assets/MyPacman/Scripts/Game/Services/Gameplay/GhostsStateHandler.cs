using ObservableCollections;
using R3;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Обрабатывает состояния персонажей
    // Переключает режимы призраков
    // Спавнит персонажа при смерти или вызывает завершение игры при недостатке очков жизни
    public class GhostsStateHandler
    {
        private readonly Dictionary<EntityType, GhostMovementService> _ghostsMap = new();           // Тут должны быть службы управляющие персонажами
        private readonly BehaviourModesFactory _behaviourModesFactory;

        public GhostsStateHandler(
            IObservableCollection<Entity> entities,
            Pacman pacman,
            TimeService timeService,
            MapHandlerService mapHandlerService,
            ILevelConfig levelConfig)
        {
            InitGhostsMap(entities, pacman, timeService, levelConfig);
            _behaviourModesFactory = new BehaviourModesFactory(mapHandlerService);

            // For test
            SetBehaviourMode(GhostBehaviorModeType.Scatter);
        }

        private void SetBehaviourMode(GhostBehaviorModeType behaviorModeType)
        {
            foreach (var ghost in _ghostsMap)
            {
                var behaviourMode = _behaviourModesFactory.CreateMode(behaviorModeType);
                ghost.Value.BindBehaviorMode(behaviourMode);

                ghost.Value.TargetReached += OnTargetReached;           // Это должно производится однократно

                if (ghost.Key != EntityType.Blinky)
                {
                    gh
                }
            }
        }

        private void InitGhostsMap(
            IObservableCollection<Entity> entities,
            Pacman pacman,
            TimeService timeService,
            ILevelConfig levelConfig)
        {
            foreach (var entity in entities)
                TryCreateMovementService(entity, pacman, timeService, levelConfig);

            entities.ObserveAdd().Subscribe(e => TryCreateMovementService(e.Value, pacman, timeService, levelConfig));
            entities.ObserveRemove().Subscribe(e => TryDestroyMovementService(e.Value.Type));
        }

        private bool TryCreateMovementService(
            Entity entity,
            Pacman pacman,
            TimeService timeService,
            ILevelConfig levelConfig)
        {
            if (CheckEntityOnGhost(entity))
            {
                _ghostsMap.Add(entity.Type, new GhostMovementService(entity as Ghost, pacman, timeService, levelConfig));
                return true;
            }

            return false;
        }

        private object TryDestroyMovementService(EntityType entityType)
        {
            if (_ghostsMap.ContainsKey(entityType))
            {
                _ghostsMap.Remove(entityType);
                return true;
            }

            return false;
        }

        private bool CheckEntityOnGhost(Entity entity)
        {
            return (entity.Type <= EntityType.Blinky && entity.Type >= EntityType.Clyde);
        }

        // For test
        private void OnTargetReached(GhostMovementService ghostMovement)
        {
            if (ghostMovement.BehaviorModeType == GhostBehaviorModeType.Scatter)    // Смена целевой точки при выходе из загона
            {
                var position = GetScatterPosition(ghostMovement.EntityType);
                ghostMovement.      // Сменить целевую точку для данного призрака
            }
        }

        private Vector2 GetScatterPosition(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Blinky:
                    return Vector2.zero;                                    // Magic

                case EntityType.Pinky:
                    return new Vector2(29f, 0f);                            // Magic

                case EntityType.Inky:
                    return new Vector2(0f, -33f);                           // Magic

                case EntityType.Clyde:
                    return new Vector2(29f, -33f);                          // Magic

                default:
                    throw new System.Exception($"There is no implementation for this type: {entityType}");      // Magic
            }
        }
    }
}
