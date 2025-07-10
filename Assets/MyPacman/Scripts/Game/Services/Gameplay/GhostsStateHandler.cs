using ObservableCollections;
using R3;
using System.Collections.Generic;

namespace MyPacman
{
    // Обрабатывает состояния персонажей
    // Переключает режимы призраков
    // Спавнит персонажа при смерти или вызывает завершение игры при недостатке очков жизни
    public class GhostsStateHandler
    {
        private readonly Dictionary<EntityType, GhostMovementService> _ghostsMap = new();           // Тут должны быть службы управляющие персонажами
        private readonly Dictionary<GhostBehaviorModeType, IGhostBehaviorMode> _ghostBehaviorModeMap = new();

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
                ghost.Value.BindBehaviorMode(_ghostBehaviorModeMap[GhostBehaviorModeType.Frightened]);
            }
        }

        private void InitGhostBehaviorModeMap(MapHandlerService mapHandlerService)
        {
            _ghostBehaviorModeMap.Add(GhostBehaviorModeType.Frightened, new BehaviourModeFrightened(mapHandlerService));
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
    }
}
