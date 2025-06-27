using UnityEngine;
using UnityEngine.InputSystem;

namespace MyPacman
{
    public class OldPacmanView : MonoBehaviour      // Выпилить
    {
        private Pacman _entity;
        private IGameStateService _gameStateService;
        private Rigidbody2D _rigidbody;
        private PlayerMovementHandler _playerMoveHandler;       // Переделать в сервис
        private PlayerInputActions _inputActions;

        private float _timer;

        private void OnDisable()
        {
            _inputActions.Disable();
            _inputActions.Keyboard.Movement.performed -= OnMoveStarted;
            _inputActions.Keyboard.Movement.canceled -= OnMoveCanceled;
        }

        public void Bind(Pacman entity,
            PlayerInputActions inputActions,
            IGameStateService gameStateService,
            IMapHandler mapHandler,
            TimeService timeService)
        {
            _entity = entity;
            _gameStateService = gameStateService;

            _rigidbody = GetComponent<Rigidbody2D>();
            _playerMoveHandler = new PlayerMovementHandler(_rigidbody, _entity);

            var mapSize = new Vector2(mapHandler.Map.GetLength(1), -mapHandler.Map.GetLength(0));
            _playerMoveHandler.Initialyze(() => _inputActions.Keyboard.Movement.ReadValue<Vector2>(), mapSize);
            _playerMoveHandler.TileChanged += mapHandler.OnPlayerTilesChanged;         // Вынести в отдельный инициализатор

            _inputActions = inputActions;
            _inputActions.Enable();
            _inputActions.Keyboard.Movement.started += OnMoveStarted;
            _inputActions.Keyboard.Movement.canceled += OnMoveCanceled;

            timeService.TimeHasTicked += Tick;
            timeService.TimeHasTicked += _playerMoveHandler.Tick;
        }

        private void OnMoveStarted(InputAction.CallbackContext context)
        {
            _playerMoveHandler.StartMoving();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _playerMoveHandler.StopMoving();
        }

        private void Tick()
        {
            if (_timer > 1f)    // Автосохранение раз в секунду, вынести в сервис сохранений                 Magic
            {
                _gameStateService.SaveGameState();
                _timer = 0f;
            }

            _timer += Time.deltaTime;
        }
    }
}
