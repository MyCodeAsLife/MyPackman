using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyPacman
{
    public class PlayerMovementService
    {
        public readonly ReactiveProperty<Vector3Int> PlayerTilePosition = new();

        private readonly TimeService _timeService;
        private readonly Pacman _entity;

        private readonly IGameStateService _gameStateService;       //For save
        private float _timer;                                       //For save

        private Vector2 _mapSize;
        private Vector2Int _lastDirection;
        private Vector2 _lastPosition;

        private Func<Vector2> GetMovementDirection;

        private event Action Moved;

        public PlayerMovementService(
            Pacman entity,
            PlayerInputActions inputActions,            // Нужно передавать или создать здесь?
            IGameStateService gameStateService,
            ILevelConfig levelConfig,
            TimeService timeService)
        {
            GetMovementDirection = inputActions.Keyboard.Movement.ReadValue<Vector2>;        // Передать сюда только функцию?
            _entity = entity;
            _gameStateService = gameStateService;
            _timeService = timeService;
            _timeService.TimeHasTicked += Tick;

            _mapSize = new Vector2(levelConfig.Map.GetLength(1), -levelConfig.Map.GetLength(0));
            PlayerTilePosition.OnNext(Convert.ToTilePosition(_entity.Position.Value));
        }

        ~PlayerMovementService()
        {
            _timeService.TimeHasTicked -= Tick;
        }

        private void Tick()
        {
            if (_timer > 1f)    // Автосохранение раз в секунду, вынести в сервис сохранений                 Magic
            {
                _gameStateService.SaveGameState();
                _timer = 0f;
            }

            _timer += Time.deltaTime;

            Moved?.Invoke();
        }

        private void Movement()
        {
            Vector2 movementDirection = GetMovementDirection();
            Vector2 currentPosition = _entity.GetCurrentPosition();

            Rotate(movementDirection);
            Move(currentPosition, movementDirection);
        }

        public void OnMoveStarted(InputAction.CallbackContext context)
        {
            _entity.IsMoving.OnNext(true);
            Moved += Movement;
        }

        public void OnMoveCanceled(InputAction.CallbackContext context)
        {
            Moved -= Movement;
            _entity.IsMoving.OnNext(false);
        }

        private void Rotate(Vector2 currentDirection)
        {
            int currentX = (int)Mathf.Round(currentDirection.x);
            int currentY = (int)Mathf.Round(currentDirection.y);
            var dir = new Vector2Int();

            if (currentX != 0 && currentY != 0)
            {
                if (currentX != _lastDirection.x)
                    dir.x = currentX;
                else if (currentY != _lastDirection.y)
                    dir.y = currentY;
                else
                    return;
            }
            else
            {
                dir.x = currentX;
                dir.y = currentY;
            }

            _entity.Direction.OnNext(dir);
            _lastDirection = new Vector2Int(currentX, currentY);
        }

        private void Move(Vector2 currentPosition, Vector2 currentDirection)
        {
            if (currentPosition != _lastPosition)
                _lastPosition = currentPosition;

            float nextPosX = MoveOnAxis(currentPosition.x, currentDirection.x);
            float nextPosY = MoveOnAxis(currentPosition.y, currentDirection.y);

            nextPosX = Utility.RepeatInRange(nextPosX, 1, _mapSize.x - 1);
            nextPosY = Utility.RepeatInRange(nextPosY, _mapSize.y + 2, 0);

            var nextPosition = new Vector2(nextPosX, nextPosY);
            var newTilePosition = Convert.ToTilePosition(nextPosition);

            if (PlayerTilePosition.Value != newTilePosition)
                PlayerTilePosition.Value = newTilePosition;

            _entity.Position.OnNext(nextPosition);
        }

        private float MoveOnAxis(float currentPositionOnAxis, float direction)
        {
            direction = Mathf.Round(direction);
            float nextPosOnAxis = currentPositionOnAxis + (GameConstants.PlayerSpeed * Time.fixedDeltaTime * direction);

            return nextPosOnAxis;
        }
    }
}