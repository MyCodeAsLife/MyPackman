using Assets.MyPackman.Settings;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.MyPackman.Presenter
{
    public class PlayerMovementHandler : IPlayerMovementHandler
    {
        private readonly Vector3Int[] DirectionOffset = new Vector3Int[4] { new Vector3Int(-ConstantsGame.MovementStep, 0, 0),
                                                                            new Vector3Int(ConstantsGame.MovementStep, 0, 0),
                                                                            new Vector3Int(0, -ConstantsGame.MovementStep, 0),
                                                                            new Vector3Int(0, ConstantsGame.MovementStep, 0) };   // Вынести в настройки??

        private Coroutine _movement;
        private Transform _transform;
        private bool[] _directionsPresed = new bool[4];
        private int _lastDirection = ConstantsGame.NoDirection;
        private IMapHandler _mapHandler;                                    // DI - ?
        private MonoBehaviour _parent;                              // Необходимо для работы корутин
        private Vector3Int _currentPosition;                       // Получить начальную позицию игрока из LevelConstruction?

        private event Action Moved;                                  // Вынести в шину событий?

        public PlayerMovementHandler(Transform transform, IMapHandler handler, MonoBehaviour parent)
        {
            _transform = transform;
            _mapHandler = handler;
            _parent = parent;

            if (_mapHandler.TryFindPositionByObjectNumber(ConstantsGame.Player, ref _currentPosition) == false)
                throw new Exception(ConstantsGame.PositionOnMapNotFound);
        }

        public void Tick()
        {
            Moved?.Invoke();
        }

        public void SetMovementDirection(int currentDirectionPresed)
        {
            _directionsPresed[currentDirectionPresed] = true;

            for (int i = 0; i < _directionsPresed.Length; i++)
                if (_directionsPresed[i] && i != currentDirectionPresed)
                    return;

            Moved += Move;
        }

        public void RemoveMovementDirection(int removedDirection)
        {
            _directionsPresed[removedDirection] = false;

            for (int i = 0; i < _directionsPresed.Length; i++)
                if (_directionsPresed[i])
                    return;

            Moved -= Move;
            _lastDirection = ConstantsGame.NoDirection;
        }

        private void Move()
        {
            if (_movement == null)
            {
                int currentDirection = GetMovementDirection();
                Vector3Int nextPosition = _currentPosition + DirectionOffset[currentDirection];
                _lastDirection = currentDirection;

                if (CanMove(nextPosition))
                {
                    _movement = _parent.StartCoroutine(Moving(nextPosition));           // Корутины в монобехе. переделать через task
                }
            }
        }

        private int GetMovementDirection()
        {
            int currentDirection = ConstantsGame.NoDirection;

            for (int i = 0; i < _directionsPresed.Length; i++)
                if (_directionsPresed[i])
                    currentDirection = i;

            if (_lastDirection != ConstantsGame.NoDirection)
                for (int i = 0; i < _directionsPresed.Length; i++)
                    if (_directionsPresed[i] && i != _lastDirection)
                        currentDirection = i;

            return currentDirection;
        }

        private IEnumerator Moving(Vector3Int nextPosition)
        {
            Vector3 correctPosition = new Vector3(nextPosition.x, -nextPosition.y, 0);

            while (_transform.position != correctPosition)
            {
                _transform.position = Vector3.MoveTowards(_transform.position, correctPosition, ConstantsGame.PlayerSpeed);
                yield return null;
            }
            // По окончанию корутины сменить местоположение игрока на карте в MapHandler (через шину событий?)
            _mapHandler.ChangeTile(_currentPosition, ConstantsGame.EmptyTile);
            _mapHandler.ChangeTile(nextPosition, ConstantsGame.Player);

            _currentPosition = nextPosition;
            _movement = null;
        }

        private bool CanMove(Vector3Int position)
        {
            int tile = _mapHandler.Tile(position);

            if (tile >= ConstantsGame.UpperLeftCornerWall && tile <= ConstantsGame.RightEndWall)
                return false;

            return true;
        }
    }
}
