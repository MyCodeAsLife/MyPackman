using ObservableCollections;
using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    // Обрабатывает состояния персонажей
    // Переключает режимы призраков
    // Спавнит персонажа при смерти или вызывает завершение игры при недостатке очков жизни
    public class GhostsStateHandler
    {
        private readonly Dictionary<EntityType, GhostMovementService> _ghostMovementServicesMap = new();// Тут должны быть службы управляющие персонажами
        private readonly Dictionary<EntityType, Ghost> _ghostsMap = new();
        private readonly BehaviourModesFactory _behaviourModesFactory;

        public GhostsStateHandler(
            IObservableCollection<Entity> entities,
            Pacman pacman,
            TimeService timeService,
            MapHandlerService mapHandlerService,
            ILevelConfig levelConfig,
            ReadOnlyReactiveProperty<Vector2> homePosition) // Или выбирать homePosition для каждого призрака отдельно?
        {
            Vector2 mapSize = new Vector2(levelConfig.Map.GetLength(1), -levelConfig.Map.GetLength(0));

            InitGhostsMap(entities);
            InitGhostMovementServicesMap(entities, pacman.Position, timeService, levelConfig);
            _behaviourModesFactory = new BehaviourModesFactory(mapHandlerService, pacman.Position, mapSize, homePosition);

            // For test
            SetBehaviourModes(GhostBehaviorModeType.Scatter);
        }

        ~GhostsStateHandler()
        {
            foreach (var movementService in _ghostMovementServicesMap.Values)
                movementService.TargetReached -= OnTargetReached;
        }

        private void SetBehaviourModes(GhostBehaviorModeType behaviorModeType)
        {
            foreach (var ghostMovementService in _ghostMovementServicesMap)
            {
                var ghost = _ghostsMap.First(ghost => ghost.Key == ghostMovementService.Key).Value;
                var behaviourMode = _behaviourModesFactory.CreateMode(behaviorModeType, ghost);
                ghostMovementService.Value.BindBehaviorMode(behaviourMode);
            }
        }

        private void InitGhostMovementServicesMap(
            IObservableCollection<Entity> entities,
            ReadOnlyReactiveProperty<Vector2> pacmanPosition,
            TimeService timeService,
            ILevelConfig levelConfig)
        {
            foreach (var entity in entities)
                TryCreateMovementService(entity, pacmanPosition, timeService, levelConfig);

            entities.ObserveAdd().Subscribe(e => TryCreateMovementService(e.Value,
                pacmanPosition,
                timeService,
                levelConfig));
            entities.ObserveRemove().Subscribe(e => TryDestroyMovementService(e.Value.Type));
        }

        private void InitGhostsMap(IObservableCollection<Entity> entities)
        {
            foreach (var entity in entities)
                if (CheckEntityOnGhost(entity))
                    _ghostsMap.Add(entity.Type, entity as Ghost);

            entities.ObserveAdd().Subscribe(e =>
            {
                if (CheckEntityOnGhost(e.Value))
                    _ghostsMap.Add(e.Value.Type, e.Value as Ghost);
            });

            entities.ObserveRemove().Subscribe(e => _ghostsMap.First(value => value.Key == e.Value.Type));
        }

        private bool TryCreateMovementService(
            Entity entity,
            ReadOnlyReactiveProperty<Vector2> pacmanPosition,
            TimeService timeService,
            ILevelConfig levelConfig)
        {
            if (CheckEntityOnGhost(entity))
            {
                var ghostMovementService = new GhostMovementService(
                    entity as Ghost,
                    pacmanPosition,
                    timeService,
                    levelConfig);
                _ghostMovementServicesMap.Add(entity.Type, ghostMovementService);
                ghostMovementService.TargetReached += OnTargetReached;
                return true;
            }

            return false;
        }

        private object TryDestroyMovementService(EntityType entityType)
        {
            if (_ghostMovementServicesMap.ContainsKey(entityType))
            {
                _ghostMovementServicesMap.Remove(entityType);
                return true;
            }

            return false;
        }

        private bool CheckEntityOnGhost(Entity entity)
        {
            return (entity.Type <= EntityType.Blinky && entity.Type >= EntityType.Clyde);
        }

        // For test
        private void OnTargetReached(EntityType entityType)     // Добавить логики вызывающие этот метод при (разбегании, преследовании, страхе, +возврате)
        {
            // 1. Преследование
            // Проверять дистаницию до цели, если достигнута то пакман съеден
            // 2. Страх
            // Проверять дистаницию до цели, если достигнута то призрак съеден
            // +3. Возврат
            // 4. Разбегание
            // Либо запускать таймер, либо при выходе из загона(как реализовать выход из загона?)
            var movementService = _ghostMovementServicesMap[entityType];
            var behaviourModeType = movementService.BehaviorModeType;

            if (behaviourModeType == GhostBehaviorModeType.Homecomming)  // Смена поведения при возврате в загон
            {
                var entity = _ghostsMap[entityType];
                var behaviourMode = _behaviourModesFactory.CreateMode(GhostBehaviorModeType.Scatter, entity);
                movementService.BindBehaviorMode(behaviourMode);
            }

            // Нужно добавить (таймер?) на переключение поведения при выходе из загона.
        }
    }
}
