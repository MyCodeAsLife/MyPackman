using ObservableCollections;
using R3;
using System;
using UnityEngine;

namespace MyPacman
{
    public class EntitiesStateHandler
    {
        private readonly ReactiveProperty<float> _levelTimeHasPassed;                       // Время с начала раунда(без пауз)
        private readonly ReactiveProperty<float> _timeLeftUntilEndOfFearMode;               // Время до окончания страха
        private readonly ReactiveProperty<float> _globalBehaviorModeChangeTimer;            // Таймер смены глобального поведения
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
            ReactiveProperty<float> timeLeftUntilEndOfFearMode,
            ReactiveProperty<GhostBehaviorModeType> globalStateOfBehavior,
            ReactiveProperty<float> globalBehaviorModeChangeTimer)
        {
            // New  Вынести в регистрацию? А сюда переавать уже созданный GhostsStateHandler и PacmanStateHandler ????
            _ghostsStateHandler = new GhostsStateHandler(entities, pacman, getSpawnPosition, timeService, mapHandlerService, levelConfig, globalStateOfBehavior);
            _pacmanStateHandler = new PacmanStateHandler(entities, pacman, pacmanLifePoints, getSpawnPosition, timeService, mapHandlerService, levelConfig);
            _timeService = timeService;
            _timeService.TimeHasTicked += Tick;
            _pacmanStateHandler.SubscribeToDeadAnimationFinish(_ghostsStateHandler.ShowGhosts);
            _levelTimeHasPassed = levelTimeHasPassed;
            _timeLeftUntilEndOfFearMode = timeLeftUntilEndOfFearMode;
            _globalStateOfBehavior = globalStateOfBehavior;
            _globalBehaviorModeChangeTimer = globalBehaviorModeChangeTimer;

            SubscribeToTargetReachingEvent();

            StartInit();

            ////// For test
            //SwitchBehaviorModes(GhostBehaviorModeType.Scatter);
        }

        private void StartInit()
        {
            // Проверяем текущее глобальное поведение
            // 1. Если страх то дожидаемся его окончания
            // 1.2 По окончанию страха переходим на пункт 2
            // 2. Если не страх то проверяем текущее время таймера смены поведения
            // 2.1. Если не равно -1(меньше нуля), то это новая игра и продолжить базовый запуск глобального поведения (включить таймер смены поведения)
            // 2.2. Если равно -1(меньше нуля), то запуск глобального поведения "перследование".
            // Таймер смены поведения должен приостанавливатся на время работы страха и сбрасыватся при смерти игрока

            if (_globalStateOfBehavior.Value == GhostBehaviorModeType.Frightened)
                Timer += TickTimeLeftUntilEndOfFearMode;
            else
                Timer += TickGlobalBehaviorModeChangeTimer;
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

        private void TickTimeLeftUntilEndOfFearMode()   // Переименовать
        {
            _timeLeftUntilEndOfFearMode.Value -= _timeService.DeltaTime;

            if (_timeLeftUntilEndOfFearMode.Value <= 0)
            {
                _globalBehaviorModeChangeTimer.Value = 0;   // Сброс глобального таймера поведения, для его перезапуска
                _timeLeftUntilEndOfFearMode.Value = 0;
                Timer -= TickTimeLeftUntilEndOfFearMode;
                Timer += TickGlobalBehaviorModeChangeTimer; // Возврат к текущему таймеру смены глобальных поведений
            }
        }

        private void TickGlobalBehaviorModeChangeTimer()    // Переименовать
        {
            // 2. Проверяем текущее время таймера смены поведения
            // Таймер смены поведения должен приостанавливатся на время работы страха и сбрасыватся при смерти игрока

            throw new NotImplementedException();

            // 2.1. Если равно 0, то это новая игра и продолжить базовый запуск глобального поведения (включить таймер смены поведения)
            if (_globalBehaviorModeChangeTimer.Value == 0)
            {

            }
            // 2.2. Если больше 0 , то это загруженная игра, проверить текущее глобальное состояние и присвоить его всем? (включить таймер смены поведения)
            else if (_globalBehaviorModeChangeTimer.Value > 0)
            {

            }
            // 2.3. Если равно -1(меньше нуля), то запуск глобального поведения "преследование" (отключить таймер смены поведения).
            else
            {

            }
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

        //// Регулировка поведения призраков
        //private void SwitchBehaviorModes(GhostBehaviorModeType behaviorModeType)    // global ghost state handler
        //{
        //    _ghostsStateHandler.GlobalStateOfGhosts = behaviorModeType;
        //    _ghostsStateHandler.SetBehaviourModeEveryone(behaviorModeType);

        //    if (behaviorModeType == GhostBehaviorModeType.Chase || behaviorModeType == GhostBehaviorModeType.Scatter)
        //    {
        //        _timeLeftUntilEndOfFearMode.Value = _ghostsStateHandler.GetTimerForBehaviorType(behaviorModeType);
        //    }
        //    else
        //    {
        //        _timeLeftUntilEndOfFearMode.Value = 3f;
        //    }

        //    //_timer = 0f;
        //    Timer += CheckTimerTest;
        //}

        //// For test
        //private void CheckTimerTest()
        //{
        //    _timeLeftUntilEndOfFearMode.Value -= _timeService.DeltaTime;

        //    if (_timeLeftUntilEndOfFearMode.Value <= 0)
        //    {
        //        _timeLeftUntilEndOfFearMode.Value = 0;

        //        if (_ghostsStateHandler.GlobalStateOfGhosts == GhostBehaviorModeType.Scatter)
        //        {
        //            _ghostsStateHandler.SetBehaviourModeEveryone(GhostBehaviorModeType.Frightened);
        //        }
        //        else if (_ghostsStateHandler.GlobalStateOfGhosts == GhostBehaviorModeType.Frightened)
        //        {
        //            _ghostsStateHandler.SetBehaviourModeEveryone(GhostBehaviorModeType.Homecomming);

        //            Timer -= CheckTimerTest;
        //        }
        //    }
        //}
    }
}