using ObservableCollections;
using R3;
using System;
using System.Collections;
using UnityEngine;

namespace MyPacman
{
    public class PacmanStateHandler
    {
        private readonly TimeService _timeService;
        private readonly Pacman _pacman;
        private readonly ReactiveProperty<int> _pacmanLifePoints;

        private float _invincibleTimer = 0f;

        private Func<SpawnPointType, Vector2> GetSpawnPosition;     // Или передать сюда сразу точку спавна игрока?

        public PacmanStateHandler(IObservableCollection<Entity> entities,
            Pacman pacman,
            ReactiveProperty<int> pacmanLifePoints,             // Регулировка поведения Игрока
            Func<SpawnPointType, Vector2> getSpawnPosition,
            TimeService timeService,
            HandlerOfPickedEntities mapHandlerService,
            ILevelConfig levelConfig)
        {
            _timeService = timeService;
            _pacman = pacman;
            _pacmanLifePoints = pacmanLifePoints;
            GetSpawnPosition = getSpawnPosition;
            _pacman.DeadAnimationFinished += OnDeadAnimationFinished;
        }

        ~PacmanStateHandler()
        {
            _pacman.DeadAnimationFinished -= OnDeadAnimationFinished;
        }

        public bool IsInvincible => _invincibleTimer > 0f;
        public Action<Action> SubscribeToDeadAnimationFinish => _pacman.SubscribeToDeadAnimationFinish;
        public Action<Action> UnsubscribeFromDeadAnimationFinish => _pacman.UnsubscribeFromDeadAnimationFinish;

        public void PacmanTakeDamage()     // Вызывать отдельный обработчик состояний игрока
        {
            _invincibleTimer = GameConstants.PlayerInvincibleTimer;
            _pacman.Dead.OnNext(Unit.Default);
            _timeService.StopTime();

            if (_pacmanLifePoints.Value > 0)
            {
                _pacmanLifePoints.Value--;
            }
            else
            {
                // если равно нулю или меньше то вызываем конец игры
                Debug.Log("Player lifes is end!");              //++++++++++++++++++++++++++++++++++++++++++++++++
            }
        }

        public void OnDeadAnimationFinished()
        {
            _pacman.Position.Value = GetSpawnPosition(SpawnPointType.Pacman);
            _timeService.RunTime();
            Coroutines.StartRoutine(InvulnerabilityTimer());
        }

        private IEnumerator InvulnerabilityTimer()     // Неуязвимость игрока.
        {
            // Включить мигание или уменьшение и увеличение прозрачности пакмана на время неуязвимости
            while (_invincibleTimer > 0)
            {
                _invincibleTimer -= _timeService.DeltaTime + 0.6f;  // сумма двух следующих задержек
                yield return new WaitForSeconds(0.3f);              // Magic - задержка в мигании (мигания еще нет)

                yield return new WaitForSeconds(0.3f);              // Magic - задержка в мигании (мигания еще нет)
            }

            _invincibleTimer = 0f;
        }
    }
}