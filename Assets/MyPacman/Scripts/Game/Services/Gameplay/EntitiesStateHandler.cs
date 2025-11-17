using ObservableCollections;
using R3;
using System;
using UnityEngine;

namespace MyPacman
{
    public class EntitiesStateHandler
    {
        private readonly GhostsStateHandler _ghostsStateHandler;
        private readonly PacmanStateHandler _pacmanStateHandler;
        private readonly TimeService _timeService;

        private float _amountTime = 0f;
        private float _timer = 0f;
        private float _levelTimeHasPassed = 0f;                 // Время с начала раунда(без пауз). Перенести в сохранения?

        private event Action Timer;

        public EntitiesStateHandler(
            IObservableCollection<Entity> entities,
            Pacman pacman,
            ReactiveProperty<int> pacmanLifePoints,             // Регулировка поведения Игрока
            Func<SpawnPointType, Vector2> getSpawnPosition,
            TimeService timeService,
            MapHandlerService mapHandlerService,
            ILevelConfig levelConfig)
        {
            // New  Вынести в регистрацию? А сюда переавать уже созданный GhostsStateHandler и PacmanStateHandler ????
            _ghostsStateHandler = new GhostsStateHandler(entities, pacman, pacmanLifePoints, getSpawnPosition, timeService, mapHandlerService, levelConfig);
            _pacmanStateHandler = new PacmanStateHandler(entities, pacman, pacmanLifePoints, getSpawnPosition, timeService, mapHandlerService, levelConfig);
            _timeService = timeService;
            _timeService.TimeHasTicked += Tick;
            _pacmanStateHandler.SubscribeToDeadAnimationFinish(_ghostsStateHandler.ShowGhosts);
            SubscribeToTargetReachingEvent();

            // For test
            SwitchBehaviorModes(GhostBehaviorModeType.Scatter);
        }

        private void SubscribeToTargetReachingEvent()
        {
            foreach (var movementService in _ghostsStateHandler.GhostMovementServicesMap.Values)
                movementService.TargetReached += OnTargetReached;
        }

        ~EntitiesStateHandler()
        {
            _timeService.TimeHasTicked -= Tick;
            _pacmanStateHandler.UnsubscribeFromDeadAnimationFinish(_ghostsStateHandler.ShowGhosts);

            foreach (var movementService in _ghostsStateHandler.GhostMovementServicesMap.Values)
                movementService.TargetReached -= OnTargetReached;
        }

        private void Tick()
        {
            _levelTimeHasPassed += _timeService.DeltaTime;
            Timer?.Invoke();
        }
        // Регулировка поведения призраков
        private void SwitchBehaviorModes(GhostBehaviorModeType behaviorModeType)
        {
            _ghostsStateHandler.GlobalStateOfGhosts = behaviorModeType;

            foreach (var ghost in _ghostsStateHandler.GhostsMap)
                _ghostsStateHandler.SetBehaviourMode(ghost.Key, behaviorModeType);

            if (behaviorModeType > GhostBehaviorModeType.Chase && behaviorModeType < GhostBehaviorModeType.Homecomming)
            {
                _timer = 0f;
                _amountTime = _ghostsStateHandler.GetTimerForBehaviorType(behaviorModeType);
                Timer += OnTimer;
            }
        }
        // For test
        private void CheckTimerTest()
        {
            if (_amountTime < _timer)
            {
                Timer -= OnTimer;

                switch (_ghostsStateHandler.GlobalStateOfGhosts)        // У каждого призрака может быть свое состояние
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

        private void OnTargetReached(EntityType entityType)
        {
            // -1. Преследование
            // -Проверять дистаницию до цели, если достигнута то пакман съеден
            // -2. Страх
            // -Проверять дистаницию до цели, если достигнута то призрак съеден
            // -3. Возврат
            // При добегании до точки(в загоне) запускать таймер - таймер будет вести призрак?
            // -4. Разбегание
            // При выходе из загона запускать таймер - таймер будет вести призрак?
            var behaviourModeType = _ghostsStateHandler.GhostMovementServicesMap[entityType].BehaviorModeType;

            if (behaviourModeType == GhostBehaviorModeType.Homecomming)  // Смена поведения при возврате в загон
            {
                _ghostsStateHandler.SetBehaviourMode(entityType, GhostBehaviorModeType.Scatter);
            }
            else if (_ghostsStateHandler.IsPacmanReached(entityType))
            {
                OnRanIntoPacman(entityType);
            }

            // Нужно добавить (таймер?) на переключение поведения при выходе из загона.
        }

        private void OnRanIntoPacman(EntityType entityType) // Призрак сообщает когда сталкивается с игроком
        {
            // Проверяем текущее состояние/поведение призрака и в зависимости от него реагируем.
            var behaviourModeType = _ghostsStateHandler.GhostMovementServicesMap[entityType].BehaviorModeType;

            if (behaviourModeType == GhostBehaviorModeType.Frightened)      // Если призрак под страхом
            {
                // Переключаем его в режим возвращения домой
                _ghostsStateHandler.SetBehaviourMode(entityType, GhostBehaviorModeType.Homecomming);
                // Увеличить кол-во очков (в зависимости от того какой по счету съеден призрак за время работы страха)
            }
            else if (behaviourModeType != GhostBehaviorModeType.Homecomming)  // Если призрак не возвращается домой
            {
                // Вызвать событие получения урона
                if (_pacmanStateHandler.IsInvincible == false)
                {
                    _pacmanStateHandler.PacmanTakeDamage();
                    _ghostsStateHandler.HideGhosts();
                }
            }
        }
    }
}