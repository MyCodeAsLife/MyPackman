using ObservableCollections;
using R3;
using System;
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
        private readonly TimeService _timeService;

        private GhostBehaviorModeType _globalStateOfGhosts;
        private float _amountTime = 0f;
        private float _timer = 0f;
        private float _levelTimeHasPassed = 0f;         // Время с начала раунда(без пауз)

        private event Action Timer;

        public GhostsStateHandler(
            IObservableCollection<Entity> entities,
            Pacman pacman,
            TimeService timeService,
            MapHandlerService mapHandlerService,
            ILevelConfig levelConfig,
            ReadOnlyReactiveProperty<Vector2> homePosition) // Или выбирать homePosition для каждого призрака отдельно?
        {
            Vector2 mapSize = new Vector2(levelConfig.Map.GetLength(1), -levelConfig.Map.GetLength(0));
            _timeService = timeService;
            _behaviourModesFactory = new BehaviourModesFactory(mapHandlerService, pacman.Position, mapSize, homePosition);
            _timeService.TimeHasTicked += Tick;

            InitGhostsMap(entities);
            InitGhostMovementServicesMap(entities, pacman.Position, levelConfig);
            SwitchBehaviorModes(GhostBehaviorModeType.Scatter);
        }

        ~GhostsStateHandler()
        {
            _timeService.TimeHasTicked -= Tick;

            foreach (var movementService in _ghostMovementServicesMap.Values)
                movementService.TargetReached -= OnTargetReached;
        }

        private void Tick()
        {
            _levelTimeHasPassed += Time.fixedDeltaTime;
            Timer?.Invoke();
        }

        private void SwitchBehaviorModes(GhostBehaviorModeType behaviorModeType)
        {
            _globalStateOfGhosts = behaviorModeType;

            foreach (var ghost in _ghostsMap)
                SetBehaviourMode(ghost.Key, behaviorModeType);

            if (behaviorModeType > GhostBehaviorModeType.Chase && behaviorModeType < GhostBehaviorModeType.Homecomming)
            {
                _timer = 0f;
                _amountTime = GetTimerForBehaviorType(behaviorModeType);
                Timer += OnTimer;
            }
        }

        // For test
        private void CheckTimerTest()
        {
            if (_amountTime < _timer)
            {
                Timer -= OnTimer;

                switch (_globalStateOfGhosts)        // У каждого призрака может быть свое состояние
                {
                    case GhostBehaviorModeType.Scatter:
                        SwitchBehaviorModes(GhostBehaviorModeType.Homecomming);
                        break;

                    case GhostBehaviorModeType.Chase:
                        SwitchBehaviorModes(GhostBehaviorModeType.Frightened);
                        break;

                    default:
                        throw new Exception(GameConstants.NoSwitchingDefined /*+ _ghostState*/);
                }
            }
        }

        private void OnTimer()
        {
            _timer += Time.fixedDeltaTime;

            CheckTimerTest();   // For tets
        }

        private float GetTimerForBehaviorType(GhostBehaviorModeType behaviorModeType)
        {
            switch (behaviorModeType)
            {
                case GhostBehaviorModeType.Scatter:
                    return GameConstants.ScatterTimer;

                case GhostBehaviorModeType.Chase:
                    return GameConstants.ChaseTimer;

                default:
                    throw new Exception(GameConstants.NoSwitchingDefined /*+ _ghostState*/);
            }
        }

        private void SetBehaviourMode(EntityType entityType, GhostBehaviorModeType behaviorModeType)
        {
            var movementService = _ghostMovementServicesMap[entityType];
            var entity = _ghostsMap[entityType];
            var behaviourMode = _behaviourModesFactory.CreateMode(behaviorModeType, entity);
            movementService.BindBehaviorMode(behaviourMode);
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
            ILevelConfig levelConfig)
        {
            if (CheckEntityOnGhost(entity))
            {
                var ghostMovementService = new GhostMovementService(
                    entity as Ghost,
                    pacmanPosition,
                    _timeService,
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

        private void OnTargetReached(EntityType entityType)     // Не срабатывает при Homecomming так как целевая точка находится между тайлами
        {
            // 1. Преследование
            // Проверять дистаницию до цели, если достигнута то пакман съеден
            // 2. Страх
            // Проверять дистаницию до цели, если достигнута то призрак съеден
            // +-3. Возврат
            // При добегании до точки(в загоне) запускать таймер - таймер будет вести призрак?
            // +-4. Разбегание
            // При выходе из загона запускать таймер - таймер будет вести призрак?
            var behaviourModeType = _ghostMovementServicesMap[entityType].BehaviorModeType;

            if (behaviourModeType == GhostBehaviorModeType.Homecomming)  // Смена поведения при возврате в загон
            {
                SetBehaviourMode(entityType, GhostBehaviorModeType.Scatter);
            }

            // Нужно добавить (таймер?) на переключение поведения при выходе из загона.
        }
    }
}