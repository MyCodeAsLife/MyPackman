using ObservableCollections;
using R3;
using System;
using System.Collections;
using UnityEngine;

namespace MyPacman
{
    public class EntitiesStateHandler
    {
        private readonly GhostsStateHandler _ghostsStateHandler;
        private readonly PacmanStateHandler _pacmanStateHandler;
        private readonly TimeService _timeService;
        private readonly ReactiveProperty<float> _levelTimeHasPassed;                 // Время с начала раунда(без пауз). Перенести в сохранения?

        //private float _amountTime = 0f;
        //private float _timer = 0f;

        private event Action Timer;

        public EntitiesStateHandler(
            IObservableCollection<Entity> entities,
            Pacman pacman,
            ReactiveProperty<int> pacmanLifePoints,             // Регулировка поведения Игрока
            Func<SpawnPointType, Vector2> getSpawnPosition,
            TimeService timeService,
            MapHandlerService mapHandlerService,
            ILevelConfig levelConfig,
            ReactiveProperty<float> levelTimeHasPassed)
        {
            // New  Вынести в регистрацию? А сюда переавать уже созданный GhostsStateHandler и PacmanStateHandler ????
            _ghostsStateHandler = new GhostsStateHandler(entities, pacman, pacmanLifePoints, getSpawnPosition, timeService, mapHandlerService, levelConfig);
            _pacmanStateHandler = new PacmanStateHandler(entities, pacman, pacmanLifePoints, getSpawnPosition, timeService, mapHandlerService, levelConfig);
            _timeService = timeService;
            _timeService.TimeHasTicked += Tick;
            _pacmanStateHandler.SubscribeToDeadAnimationFinish(_ghostsStateHandler.ShowGhosts);
            _levelTimeHasPassed = levelTimeHasPassed;

            SubscribeToTargetReachingEvent();

            //// For test
            _ghostsStateHandler.SetBehaviourModeEveryone(GhostBehaviorModeType.Scatter);
            //Coroutines.StartRoutine(RandomSwitching());
        }

        ~EntitiesStateHandler()
        {
            _timeService.TimeHasTicked -= Tick;
            _pacmanStateHandler.UnsubscribeFromDeadAnimationFinish(_ghostsStateHandler.ShowGhosts);

            foreach (var movementService in _ghostsStateHandler.GhostMovementServicesMap.Values)
                movementService.TargetReached -= OnTargetReached;
        }

        private void SubscribeToTargetReachingEvent()
        {
            foreach (var movementService in _ghostsStateHandler.GhostMovementServicesMap.Values)
                movementService.TargetReached += OnTargetReached;
        }

        private void Tick()
        {
            _levelTimeHasPassed.Value += _timeService.DeltaTime;
            Timer?.Invoke();
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

        private IEnumerator RandomSwitching()
        {
            while (true)
            {
                float delay = UnityEngine.Random.Range(2f, 3f);
                yield return new WaitForSeconds(delay);
                var newBehaviorModeType = (GhostBehaviorModeType)UnityEngine.Random.Range(0, 4);
                _ghostsStateHandler.SetBehaviourModeEveryone(newBehaviorModeType);
            }
        }

        //// Регулировка поведения призраков
        //private void SwitchBehaviorModes(GhostBehaviorModeType behaviorModeType)
        //{
        //    _ghostsStateHandler.GlobalStateOfGhosts = behaviorModeType;

        //    foreach (var ghost in _ghostsStateHandler.GhostsMap)
        //        _ghostsStateHandler.SetBehaviourMode(ghost.Key, behaviorModeType);

        //    //if (behaviorModeType > GhostBehaviorModeType.Chase && behaviorModeType < GhostBehaviorModeType.Homecomming)
        //    //{
        //    //    _timer = 0f;
        //    //    _amountTime = _ghostsStateHandler.GetTimerForBehaviorType(behaviorModeType);
        //    //    Timer += CheckTimerTest;
        //    //}
        //}

        //// For test
        //private void CheckTimerTest()
        //{
        //    _timer += _timeService.DeltaTime;

        //    if (_amountTime < _timer)
        //    {
        //        Timer -= CheckTimerTest;

        //        switch (_ghostsStateHandler.GlobalStateOfGhosts)        // У каждого призрака может быть свое состояние
        //        {
        //            case GhostBehaviorModeType.Scatter:
        //                SwitchBehaviorModes(GhostBehaviorModeType.Chase);
        //                break;

        //            case GhostBehaviorModeType.Chase:
        //                SwitchBehaviorModes(GhostBehaviorModeType.Frightened);
        //                break;

        //            default:
        //                throw new Exception(GameConstants.NoSwitchingDefined /*+ _ghostState*/);
        //        }
        //    }
        //}
        //// For test
        //// Нормальное переключение между состояниями
        //private IEnumerator RandomSwitching()   // Вынести в EntitiesStateHandler
        //{
        //    _lastBehaviorModeType = 0;

        //    while (true)
        //    {
        //        float delay = Random.Range(0f, 3f);
        //        yield return new WaitForSeconds(delay);
        //        _currentBehaviorModeType = (GhostBehaviorModeType)Random.Range(0, 4);

        //        if ((int)_currentBehaviorModeType == 1)
        //            continue;

        //        if ((int)_lastBehaviorModeType == 2) // Если был Страх
        //        {
        //            if ((int)_currentBehaviorModeType == 0 || (int)_currentBehaviorModeType == 3)       // Преследование
        //            {
        //                _eyes.enabled = true;
        //                _animatorBody.SetInteger(BehaviorModeType, (int)_currentBehaviorModeType);
        //                _lastBehaviorModeType = _currentBehaviorModeType;
        //            }
        //        }
        //        else
        //        {
        //            if ((int)_currentBehaviorModeType == 2 && (int)_lastBehaviorModeType != 3) // Страх
        //            {
        //                _eyes.enabled = false;
        //                _animatorBody.SetInteger(BehaviorModeType, (int)_currentBehaviorModeType);
        //                _lastBehaviorModeType = _currentBehaviorModeType;
        //            }
        //            else if ((int)_currentBehaviorModeType != 2)  // Возвращение домой
        //            {
        //                _eyes.enabled = true;
        //                _animatorBody.SetInteger(BehaviorModeType, (int)_currentBehaviorModeType);
        //                _lastBehaviorModeType = _currentBehaviorModeType;
        //            }
        //        }

        //    }

        //    // Old
        //    // 0 - Преследование
        //    // 1 - Разбегание
        //    // 2 - Страх
        //    // 3 - Возвращение домой

        //    //// New Переделать на это?
        //    //// 0 - Отсутствует
        //    //// 1 - Преследование
        //    //// 2 - Разбегание
        //    //// 3 - Страх
        //    //// 4 - Возвращение домой
        //}
    }
}