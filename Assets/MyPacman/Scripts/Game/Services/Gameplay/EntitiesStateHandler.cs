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
        private readonly ReactiveProperty<float> _levelTimeHasPassed;                 // Время с начала раунда(без пауз). Перенести в сохранения?

        private float _amountTime = 0f;
        private float _timer = 0f;

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
            //_ghostsStateHandler.SetBehaviourModeEveryone(GhostBehaviorModeType.Scatter);
            SwitchBehaviorModes(GhostBehaviorModeType.Scatter);
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

        // Регулировка поведения призраков
        private void SwitchBehaviorModes(GhostBehaviorModeType behaviorModeType)
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


                //switch (_ghostsStateHandler.GlobalStateOfGhosts)        // У каждого призрака может быть свое состояние
                //{
                //    case GhostBehaviorModeType.Scatter:
                //        SwitchBehaviorModes(GhostBehaviorModeType.Chase);
                //        break;

                //    case GhostBehaviorModeType.Chase:
                //        SwitchBehaviorModes(GhostBehaviorModeType.Frightened);
                //        break;

                //    default:
                //        throw new Exception(GameConstants.NoSwitchingDefined /*+ _ghostState*/);
                //}

                //_ghostsStateHandler.SetBehaviourModeEveryone(GhostBehaviorModeType.Homecomming);


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