using ObservableCollections;
using R3;
using System;
using UnityEngine;

namespace MyPacman
{
    public class EntitiesStateHandler
    {
        private readonly ReactiveProperty<float> _levelTimeHasPassed;                       // Время с начала раунда(без пауз)
        private readonly ReactiveProperty<float> _timeUntilEndOfGlobalBehaviorMode;         // Время до окончания текущего глобального состояния поведения
        private readonly ReactiveProperty<GhostBehaviorModeType> _globalStateOfBehavior;    // Текущее глобальное состояние поведения
        private readonly GhostsStateHandler _ghostsStateHandler;
        private readonly PacmanStateHandler _pacmanStateHandler;
        private readonly TimeService _timeService;

        private int _numberOfGhostsEaten = 0;   // Сбрасывать по окончанию режима глобального страха или при запуске?
        private float _amountTime = 0f;
        private float _timer = 0f;

        private event Action Timer;

        public event Action<int, Vector2> EntityEaten;

        public EntitiesStateHandler(
            IObservableCollection<Entity> entities,
            Pacman pacman,
            ReactiveProperty<int> pacmanLifePoints,             // Регулировка поведения Игрока
            Func<SpawnPointType, Vector2> getSpawnPosition,
            TimeService timeService,
            PickableEntityHandler mapHandlerService,
            ILevelConfig levelConfig,
            ReactiveProperty<float> levelTimeHasPassed,
            ReactiveProperty<float> timeUntilEndOfGlobalBehaviorMode,
            ReactiveProperty<GhostBehaviorModeType> globalStateOfBehavior)
        {
            // New  Вынести в регистрацию? А сюда переавать уже созданный GhostsStateHandler и PacmanStateHandler ????
            _ghostsStateHandler = new GhostsStateHandler(entities, pacman, getSpawnPosition, timeService, mapHandlerService, levelConfig);
            _pacmanStateHandler = new PacmanStateHandler(entities, pacman, pacmanLifePoints, getSpawnPosition, timeService, mapHandlerService, levelConfig);
            _timeService = timeService;
            _timeService.TimeHasTicked += Tick;
            _pacmanStateHandler.SubscribeToDeadAnimationFinish(_ghostsStateHandler.ShowGhosts);
            _levelTimeHasPassed = levelTimeHasPassed;
            _timeUntilEndOfGlobalBehaviorMode = timeUntilEndOfGlobalBehaviorMode;
            _globalStateOfBehavior = globalStateOfBehavior;

            SubscribeToTargetReachingEvent();

            StartInit();

            //// For test
            SwitchBehaviorModes(GhostBehaviorModeType.Scatter);
        }

        private void StartInit()
        {
            // 1. Проверяем текущее время раунда
            // 1.1. Если равно нулю то это новая игра и продолжить базовый запуск таймера глобального поведения
            // 1.2. Если неравно нулю
            // 1.2.1. На основе времени расчитать предпологаемое поведение
            // 1.2.2. Проверить текущее поведение и таймер его окончания
        }

        ~EntitiesStateHandler()
        {
            _timeService.TimeHasTicked -= Tick;
            _pacmanStateHandler.UnsubscribeFromDeadAnimationFinish(_ghostsStateHandler.ShowGhosts);

            foreach (var movementService in _ghostsStateHandler.GhostMovementServicesMap.Values)
                movementService.TargetReached -= OnTargetReached;
        }

        private void SubscribeToTargetReachingEvent()    // ghostsStateHandler ?
        {
            foreach (var movementService in _ghostsStateHandler.GhostMovementServicesMap.Values)
                movementService.TargetReached += OnTargetReached;
        }

        private void Tick()
        {
            _levelTimeHasPassed.Value += _timeService.DeltaTime;
            Timer?.Invoke();
        }

        private void OnTargetReached(EntityType entityType, Vector2 position)     // ghostsStateHandler ?
        {
            // 1. Проверка поведения
            var behaviourModeType = _ghostsStateHandler.GhostMovementServicesMap[entityType].BehaviorModeType;
            // 1.1. Если возврат домой, то проверка расстояния до "дома".(если true то переключение в разбегание иначе ничего не делать)
            // 1.2. Если страх, то проверка до игрока. (если true то событие поедания иначе ничего)
            // 1.3. Если все остальное, то проверка до игрока. (если true то попытатся нанести урон игроку)

            //if (behaviourModeType == GhostBehaviorModeType.Homecomming)
            //{
            //    if()
            //}
            //--------------------------------------------------------------------------------------------------
            if (behaviourModeType == GhostBehaviorModeType.Homecomming)  // Смена поведения при возврате в загон
            {
                _ghostsStateHandler.SetBehaviourMode(entityType, GhostBehaviorModeType.Scatter);
            }
            else if (_ghostsStateHandler.IsPacmanReached(entityType))
            {
                OnRanIntoPacman(entityType, position);
            }
        }

        private void OnRanIntoPacman(EntityType entityType, Vector2 position)
        {
            // Проверяем текущее состояние/поведение призрака и в зависимости от него реагируем.
            var behaviourModeType = _ghostsStateHandler.GhostMovementServicesMap[entityType].BehaviorModeType;

            if (behaviourModeType == GhostBehaviorModeType.Frightened)      // Если призрак под страхом
            {
                // Переключаем его в режим возвращения домой
                _ghostsStateHandler.SetBehaviourMode(entityType, GhostBehaviorModeType.Homecomming);
                // Увеличить кол-во очков (в зависимости от того какой по счету съеден призрак за время работы страха)
                _numberOfGhostsEaten++;
                int score = (int)EdibleEntityPoints.Ghost * _numberOfGhostsEaten;
                EntityEaten?.Invoke(score, position);
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

        // Регулировка поведения призраков
        private void SwitchBehaviorModes(GhostBehaviorModeType behaviorModeType)    // global ghost state handler
        {
            _ghostsStateHandler.GlobalStateOfGhosts = behaviorModeType;
            _ghostsStateHandler.SetBehaviourModeEveryone(behaviorModeType);

            if (behaviorModeType == GhostBehaviorModeType.Chase || behaviorModeType == GhostBehaviorModeType.Scatter)
            {
                _amountTime = _ghostsStateHandler.GetTimerForBehaviorType(behaviorModeType);
            }
            else
            {
                _amountTime = 3f;
            }

            _timer = 0f;
            Timer += CheckTimerTest;
        }

        // For test
        private void CheckTimerTest()
        {
            _timer += _timeService.DeltaTime;

            if (_amountTime < _timer)
            {
                if (_ghostsStateHandler.GlobalStateOfGhosts == GhostBehaviorModeType.Scatter)
                {
                    _ghostsStateHandler.SetBehaviourModeEveryone(GhostBehaviorModeType.Frightened);

                    _timer = 0f;
                }
                else if (_ghostsStateHandler.GlobalStateOfGhosts == GhostBehaviorModeType.Frightened)
                {
                    _ghostsStateHandler.SetBehaviourModeEveryone(GhostBehaviorModeType.Homecomming);

                    Timer -= CheckTimerTest;
                }
            }
        }
    }
}