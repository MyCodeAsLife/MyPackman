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
        private readonly Dictionary<EntityType, GhostMovementService> _ghostMovementServicesMap = new();// Тут должны быть службы управляющие персонажами
        private readonly Dictionary<EntityType, Ghost> _ghostsMap = new();
        private readonly BehaviourModesFactory _behaviourModesFactory;
        private readonly TimeService _timeService;
        private readonly Pacman _pacman;
        private readonly ReactiveProperty<int> _pacmanLifePoints;
        private readonly ReadOnlyReactiveProperty<Vector2> _pacmanSpawnPosition;

        private GhostBehaviorModeType _globalStateOfGhosts;
        private float _amountTime = 0f;
        private float _timer = 0f;
        private float _levelTimeHasPassed = 0f;         // Время с начала раунда(без пауз)
        private float _invincibleTimer = 0f;

        private event Action Timer;

        public GhostsStateHandler(
            IObservableCollection<Entity> entities,
            Pacman pacman,
            ReactiveProperty<int> pacmanLifePoints,
            ReadOnlyReactiveProperty<Vector2> pacmanSpawnPosition,
            TimeService timeService,
            MapHandlerService mapHandlerService,
            ILevelConfig levelConfig,
            ReadOnlyReactiveProperty<Vector2> ghostsHomePosition) // Или выбирать homePosition для каждого призрака отдельно?
        {
            _pacman = pacman;
            _pacmanLifePoints = pacmanLifePoints;
            _pacmanSpawnPosition = pacmanSpawnPosition;
            Vector2 mapSize = new Vector2(levelConfig.Map.GetLength(1), -levelConfig.Map.GetLength(0));
            _timeService = timeService;
            _timeService.TimeHasTicked += Tick;
            var blinkyPosition = entities.First(e => e.Type == EntityType.Blinky).Position;
            _behaviourModesFactory = new BehaviourModesFactory(
                mapHandlerService,
                blinkyPosition,
                pacman.Position,
                pacman.Direction,
                mapSize,
                ghostsHomePosition);

            InitGhostsMap(entities);
            InitGhostMovementServicesMap(entities, pacman.Position, levelConfig);

            // For test
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
            _levelTimeHasPassed += _timeService.DeltaTime;
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
                        SwitchBehaviorModes(GhostBehaviorModeType.Chase);
                        break;

                    case GhostBehaviorModeType.Chase:
                        SwitchBehaviorModes(GhostBehaviorModeType.Frightened);
                        break;

                    default:
                        throw new Exception(GameConstants.NoSwitchingDefined /*+ _ghostState*/);
                }
            }
        }

        // For test?
        private void OnTimer()
        {
            _timer += _timeService.DeltaTime;

            CheckTimerTest();   // For test
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

        private void OnTargetReached(EntityType entityType)
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

        // Призрак сообщает когда сталкивается с игроком
        private void OnRanIntoPacman(EntityType entityType)
        {
            if (_invincibleTimer != 0)
                return;

            // Проверяем текущее состояние/поведение призрака и в зависимости от него реагируем.
            var behaviourModeType = _ghostMovementServicesMap[entityType].BehaviorModeType;

            if (behaviourModeType == GhostBehaviorModeType.Frightened)      // Если призрак под страхом
            {
                // Переключаем его в режим возвращения домой
                SetBehaviourMode(entityType, GhostBehaviorModeType.Homecomming);
                // Увеличить кол-во очков (в зависимости от того какой по счету съеден призрак за время работы страха)
            }
            else if (behaviourModeType != GhostBehaviorModeType.Homecomming)  // Если призрак не возвращается домой
            {
                // Вызвать событие получения урона
                PacmanTakeDamage();
            }
        }

        private void PacmanTakeDamage()
        {
            // Проверяем кол-во жизней пакмана
            if (_pacmanLifePoints.Value > 0)
            {
                // если больше нуля то вычесть одну
                // телепортировать на точку спавна
                // запустить временную неуязвимость
                _pacmanLifePoints.Value--;
                _pacman.Position.Value = _pacmanSpawnPosition.CurrentValue;
                _invincibleTimer = GameConstants.PlayerInvincibleTimer;
                Timer += InvincibleTimer;
            }
            else
            {
                // если равно нулю или меньше то вызываем конец игры
            }

        }

        private void InvincibleTimer()
        {
            if (_invincibleTimer > 0)
            {
                _invincibleTimer -= _timeService.DeltaTime;
            }
            else
            {
                _invincibleTimer = 0f;
                Timer -= InvincibleTimer;
            }
        }
    }
}