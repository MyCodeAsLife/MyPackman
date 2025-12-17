using ObservableCollections;
using R3;
using System;
using UnityEngine;

namespace MyPacman
{
    public class EntitiesStateHandler
    {
        private readonly ReactiveProperty<int> _waveNumber;                     // Время до окончания страха
        private readonly ReactiveProperty<float> _levelTimeHasPassed;           // Время с начала раунда(без пауз)
        private readonly ReactiveProperty<float> _behaviorStateTimer;           // Таймер смены глобального поведения
        private readonly ReactiveProperty<float> _frightenedStateTimer;         // Таймер окончания страха
        private readonly ReactiveProperty<GhostBehaviorModeType> _globalStateOfBehavior;    // Текущее глобальное состояние поведения
        private readonly GhostsStateHandler _ghostsStateHandler;    // Нужен или обращатся напрямую к _ghostsStateHandler.GlobalStateOfGhosts?
        private readonly PacmanStateHandler _pacmanStateHandler;
        private readonly TimeService _timeService;

        private int _numberOfGhostsEaten = 0;   // Сбрасывать по окончанию режима глобального страха или при запуске? Перенести в сохранение

        public event Action<int, Vector2> EntityEaten;

        private event Action Timer;

        public EntitiesStateHandler(
            IObservableCollection<Entity> entities,
            Pacman pacman,
            ReactiveProperty<int> pacmanLifePoints,             // Регулировка поведения Игрока
            Func<SpawnPointType, Vector2> getSpawnPosition,
            TimeService timeService,
            PickableEntityHandler mapHandlerService,
            ILevelConfig levelConfig,
            ReactiveProperty<float> levelTimeHasPassed,
            ReactiveProperty<int> waveNumber,
            ReactiveProperty<GhostBehaviorModeType> globalStateOfBehavior,
            ReactiveProperty<float> behaviorStateTimer,
            ReactiveProperty<float> frightenedStateTimer)
        {
            // New  Вынести в регистрацию? А сюда переавать уже созданный GhostsStateHandler и PacmanStateHandler ????
            _ghostsStateHandler = new GhostsStateHandler(entities, pacman, getSpawnPosition, timeService, mapHandlerService, levelConfig, globalStateOfBehavior);
            _pacmanStateHandler = new PacmanStateHandler(entities, pacman, pacmanLifePoints, getSpawnPosition, timeService, mapHandlerService, levelConfig);
            _timeService = timeService;
            _timeService.TimeHasTicked += Tick;
            _pacmanStateHandler.SubscribeToDeadAnimationFinish(_ghostsStateHandler.ShowGhosts);
            _levelTimeHasPassed = levelTimeHasPassed;
            _waveNumber = waveNumber;
            _globalStateOfBehavior = globalStateOfBehavior;
            _behaviorStateTimer = behaviorStateTimer;
            _frightenedStateTimer = frightenedStateTimer;

            SubscribeToTargetReachingEvent();
            InitialBehaviorCheck();
        }

        private void InitialBehaviorCheck()
        {
            if (_frightenedStateTimer.Value > 0)
                Timer += HandleFrightenedTimer;
            else
                Timer += HandleGlobalBehaviorTimer;
        }

        ~EntitiesStateHandler()
        {
            _timeService.TimeHasTicked -= Tick;
            _pacmanStateHandler.UnsubscribeFromDeadAnimationFinish(_ghostsStateHandler.ShowGhosts);

            foreach (var movementService in _ghostsStateHandler.GhostMovementServicesMap.Values)
            {
                movementService.TargetReached -= OnTargetReached;
                movementService.CollidedWithPacman -= OnRanIntoPacman;
            }
        }

        private void SubscribeToTargetReachingEvent()    // ghostsStateHandler ?
        {
            foreach (var movementService in _ghostsStateHandler.GhostMovementServicesMap.Values)
            {
                movementService.TargetReached += OnTargetReached;
                movementService.CollidedWithPacman += OnRanIntoPacman;
            }
        }

        private void Tick()
        {
            _levelTimeHasPassed.Value += _timeService.DeltaTime;
            Timer?.Invoke();
        }

        private void HandleGlobalBehaviorTimer()
        {
            _behaviorStateTimer.Value -= _timeService.DeltaTime;

            if (_behaviorStateTimer.Value < 0)
                HandleGlobalBehaviorChangeTimer();
        }

        private void HandleFrightenedTimer()
        {
            _frightenedStateTimer.Value -= _timeService.DeltaTime;

            if (_frightenedStateTimer.Value < 0)
                HandleEndOfFrightenedState();
        }

        private void HandleEndOfFrightenedState()
        {
            _frightenedStateTimer.Value = 0;
            Timer -= HandleEndOfFrightenedState;
            Timer += HandleGlobalBehaviorChangeTimer; // Возврат к текущему таймеру смены глобальных поведений
        }

        private void HandleGlobalBehaviorChangeTimer()
        {
            Timer -= HandleGlobalBehaviorTimer;
            _behaviorStateTimer.Value = 0;
            // 2. Проверяем текущее время таймера смены поведения
            // Таймер смены поведения должен приостанавливатся на время работы страха и сбрасыватся при смерти игрока

            // 2.1. Если равно 0, последнее поведение завершилось или не начиналось
            _waveNumber.Value++;
            _globalStateOfBehavior.Value = GetGlobalStateOfBehavior();      // Получить тип поведения опираясь на текущую волну
            float time = GetTimeForBehavior(_globalStateOfBehavior.Value);  // Запросить время для текущего поведения
            time = AdjustingTimeOfBehavior(time);                           // Скоректировать время относительно текущей волны и уровня
            _behaviorStateTimer.Value = time;

            // 2.3. Если равно -1(меньше нуля), то запуск глобального поведения "преследование" (отключить таймер смены поведения).
            if (_behaviorStateTimer.Value < 0)
                _globalStateOfBehavior.Value = GhostBehaviorModeType.Chase;
            else // 2.2. Если больше 0, то это загруженная игра, включить таймер смены поведения.
                Timer += HandleGlobalBehaviorTimer;
        }

        private GhostBehaviorModeType GetGlobalStateOfBehavior()    // Проверка на кратность (чтобы узнать разбегание сейчас или преследование)
        {
            return _waveNumber.Value % 2 == 0 ? GhostBehaviorModeType.Chase : GhostBehaviorModeType.Scatter;   // Magic
        }

        private float AdjustingTimeOfBehavior(float time)
        {
            // Доделать алгоритм, рассчитывать время учитывая номер уровня
            switch (_waveNumber.Value)
            {
                case 1:
                    return time;
                case 2:
                    return time;
                case 3:
                    return time;
                case 4:
                    return time;
                case 5:
                    return time - 2;    // Magic    // 5 сек.
                case 6:
                    return time;
                case 7:
                    return time - 2;    // Magic    //  5 сек.
                default:
                    return -1;          // Magic    // Бесконечная длительность
            }
        }

        private float GetTimeForBehavior(GhostBehaviorModeType behaviorMode)
        {
            switch (behaviorMode)
            {
                case GhostBehaviorModeType.Chase:
                    return 20f;                             // Magic

                case GhostBehaviorModeType.Scatter:
                    return 7f;                              // Magic

                case GhostBehaviorModeType.Frightened:
                    return 7f;                              // Magic

                default:
                    throw new Exception($"For the \"{behaviorMode}\" behavior type, the duration of action is not defined");         // Magic
            }
        }

        private void OnTargetReached(EntityType entityType, Vector2 position)
        {
            var behaviourModeType = _ghostsStateHandler.GhostMovementServicesMap[entityType].BehaviorModeType;

            if (behaviourModeType == GhostBehaviorModeType.Homecomming)  // Смена поведения при возврате в загон
                _ghostsStateHandler.SetBehaviourMode(entityType, GhostBehaviorModeType.Scatter);
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
    }
}