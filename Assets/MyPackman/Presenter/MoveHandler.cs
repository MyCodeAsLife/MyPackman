using Assets.MyPackman.Settings;
using System.Collections;
using UnityEngine;

namespace Assets.MyPackman.Presenter
{
    class MoveHandler
    {
        private readonly Vector3[] DirectionOffset = new Vector3[4] { new Vector3(-GameSettings.MovementStep, 0f, 0f),
                                                                  new Vector3(GameSettings.MovementStep, 0f, 0f),
                                                                  new Vector3(0f, GameSettings.MovementStep, 0f),
                                                                  new Vector3(0f, -GameSettings.MovementStep, 0f) };   // Вынести в настройки??

        //private LevelMap _map = new LevelMap();             // Через DI получить?
        private Coroutine _movement;
        private Transform _transform;
        private bool[] _directionsPresed = new bool[4];
        private int _lastDirection = GameSettings.NoDirection;

        private MapHandler _handler;                                    // DI - ?

        public MoveHandler(Transform transform, MapHandler handler)
        {
            _transform = transform;
            _handler = handler;
        }

        private void Move()
        {
            if (_movement == null)
            {
                int currentDirection = GameSettings.NoDirection;

                for (int i = 0; i < _directionsPresed.Length; i++)
                    if (_directionsPresed[i])
                        currentDirection = i;

                if (_lastDirection != GameSettings.NoDirection)
                    for (int i = 0; i < _directionsPresed.Length; i++)
                        if (_directionsPresed[i] && i != _lastDirection)
                            currentDirection = i;

                var nextPosition = _transform.position + DirectionOffset[currentDirection];
                _lastDirection = currentDirection;

                if (CanMove(nextPosition))
                {
                    _movement = StartCoroutine(Moving(nextPosition));           // Корутины в монобехе. переделать через task
                }
            }
        }

        private IEnumerator Moving(Vector3 nextPosition)
        {
            while (_transform.position != nextPosition)
            {
                _transform.position = Vector3.MoveTowards(_transform.position, nextPosition, GameSettings.PlayerSpeed);
                yield return null;
            }
            // По окончанию корутины сменить местоположение игрока на карте в MapHandler (через шину событий?)
            _movement = null;
        }

        public bool CanMove(Vector3Int position)
        {
            int cell = _map.Map[-position.y, position.x];

            if (cell >= GameSettings.UpperLeftCornerWall && cell < GameSettings.RightEndWall)
                return false;

            return true;
        }
    }
}
