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
        private readonly Dictionary<GhostBehaviorModeType, GhostBehaviorMode> _ghostBehaviorModeMap = new();    // Чуш, нужно создавать новую а не раздавать всем одну и туже.

        public GhostsStateHandler(
            IObservableCollection<Entity> entities,
            Pacman pacman,
            TimeService timeService,
            MapHandlerService mapHandlerService,
            ILevelConfig levelConfig)
        {
            InitGhostsMap(entities, pacman, timeService, levelConfig);
            InitGhostBehaviorModeMap(mapHandlerService);

            // For test
            foreach (var ghost in _ghostsMap)
            {
                ghost.Value.BindBehaviorMode(_ghostBehaviorModeMap[GhostBehaviorModeType.Scatter]);     // Чуш, нужно создавать новую а не раздавать всем одну и туже.
                ghost.Value.TargetReached += OnTargetReached;

                if (ghost.Key != EntityType.Blinky)
                {
                    gh
                }
            }
        }

        private void InitGhostBehaviorModeMap(MapHandlerService mapHandlerService)
        {
            _ghostBehaviorModeMap.Add(GhostBehaviorModeType.Frightened, new BehaviourModeFrightened(mapHandlerService));
            _ghostBehaviorModeMap.Add(GhostBehaviorModeType.Scatter, new BehaviourModeScatter(mapHandlerService));
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
            if (ghostMovement.BehaviorModeType == GhostBehaviorModeType.Scatter)
            {
                int rand = Random.Range(0, 4);
                var position = GetScatterPosition(rand);
            }
        }

        private Vector2 GetScatterPosition(int numPosition)
        {
            switch (numPosition)
            {
                case 0:
                    return Vector2.zero;

                case 1:
                    return new Vector2(29f, 0f);

                case 2:
                    return new Vector2(0f, -33f);

                case 3:
                    return new Vector2(29f, -33f);

                default:
                    throw new System.Exception($"Unknown num of position: {numPosition}");
            }
        }
    }
}
