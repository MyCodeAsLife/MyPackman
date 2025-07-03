using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyPacman
{
    public class PlayerMovemenService
    {
        private PlayerInputActions _inputActions;
        private Pacman _entity;
        private TimeService _timeService;

        private IGameStateService _gameStateService;            //For save
        private float _timer;                                   //For save

        private Vector3Int _currentTilePosition;
        private Vector2 _mapSize;

        public event Action<Vector3Int> TileChanged;

        private event Action Moved;

        ~PlayerMovemenService()
        {
            _inputActions.Disable();
            _inputActions.Keyboard.Movement.performed -= OnMoveStarted;
            _inputActions.Keyboard.Movement.canceled -= OnMoveCanceled;

            _timeService.TimeHasTicked -= Tick;
        }

        public void Run(Pacman entity,
            //PlayerInputActions inputActions,            // Нужно передавать или создать здесь?
            IGameStateService gameStateService,
            ILevelConfig levelConfig,
            MapHandlerService mapHandler,
            TimeService timeService)
        {
            _entity = entity;
            _gameStateService = gameStateService;
            _timeService = timeService;

            //_mapSize = new Vector2(mapHandler.Map.GetLength(1), -mapHandler.Map.GetLength(0));
            _mapSize = new Vector2(levelConfig.Map.GetLength(1), -levelConfig.Map.GetLength(0));
            //TileChanged += mapHandler.OnPlayerTilesChanged;         // Вынести в отдельный инициализатор?

            _inputActions = new PlayerInputActions();
            _inputActions.Enable();
            _inputActions.Keyboard.Movement.started += OnMoveStarted;
            _inputActions.Keyboard.Movement.canceled += OnMoveCanceled;

            _timeService.TimeHasTicked += Tick;
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

        private void Move()
        {
            Vector2 currentDirection = _inputActions.Keyboard.Movement.ReadValue<Vector2>();
            Vector2 currentPosition = _entity.GetCurrentPosition();

            float nextPosX = MoveOnAxis(currentPosition.x, currentDirection.x);
            float nextPosY = MoveOnAxis(currentPosition.y, currentDirection.y);

            nextPosX = RepeatInRange(nextPosX, 1, _mapSize.x - 1);
            nextPosY = RepeatInRange(nextPosY, _mapSize.y + 2, 0);

            var nextPosition = new Vector2(nextPosX, nextPosY);
            var newTilePosition = MapHandler.ConvertToTilePosition(nextPosition);               // Метод вынести в утилиты как статик

            if (_currentTilePosition != newTilePosition)
            {
                _currentTilePosition = newTilePosition;
                //_entity.TilePosition.OnNext(newTilePosition);
                TileChanged?.Invoke(newTilePosition);
            }

            _entity.Position.OnNext(nextPosition);
            _entity.Direction.OnNext(currentDirection);
        }

        private void OnMoveStarted(InputAction.CallbackContext context) => Moved += Move;
        private void OnMoveCanceled(InputAction.CallbackContext context) => Moved -= Move;

        private float RepeatInRange(float value, float min, float max)
        {
            if (value < min)
                return max;
            else if (value > max)
                return min;

            return value;
        }

        private float MoveOnAxis(float currentPositionOnAxis, float direction)
        {
            direction = Mathf.Round(direction);
            float nextPosOnAxis = currentPositionOnAxis + (GameConstants.PlayerSpeed * Time.fixedDeltaTime * direction);

            return nextPosOnAxis;
        }
    }
}