using UnityEngine;
using UnityEngine.InputSystem;

namespace MyPacman
{
    public class OldPacman : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        private PlayerInputActions _inputActions;
        private IPlayerMovementHandler _playerMoveHandler;
        private TimeService _timeService;
        private IMapHandler _mapHandler;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerMoveHandler = new PlayerMovementHandler(_rigidbody, null);

            //_inputActions = new PlayerInputActions();
            //_inputActions.Enable();
            //_inputActions.Keyboard.Movement.started += OnMoveStarted;
            //_inputActions.Keyboard.Movement.canceled += OnMoveCanceled;
        }

        private void OnDisable()
        {
            _inputActions.Disable();
            _inputActions.Keyboard.Movement.performed -= OnMoveStarted;
            _timeService.TimeHasTicked -= _playerMoveHandler.Tick;
        }

        //private void Update()
        //{
        //    _playerMoveHandler.Tick();
        //}

        public void Initialize(IMapHandler mapHandler, PlayerInputActions inputActions, TimeService timeService)
        {
            _timeService = timeService;
            _mapHandler = mapHandler;
            var mapSize = new Vector2(_mapHandler.Map.GetLength(1), -_mapHandler.Map.GetLength(0));

            //_playerMoveHandler.Initialyze(() => _inputActions.Keyboard.Movement.ReadValue<Vector2>(), mapSize);
            _playerMoveHandler.TileChanged += _mapHandler.OnPlayerTilesChanged;         // Вынести в отдельный инициализатор

            _inputActions = inputActions;
            _inputActions.Enable();
            _inputActions.Keyboard.Movement.started += OnMoveStarted;
            _inputActions.Keyboard.Movement.canceled += OnMoveCanceled;

            _timeService.TimeHasTicked += _playerMoveHandler.Tick;
        }

        private void OnMoveStarted(InputAction.CallbackContext context)
        {
            _playerMoveHandler.StartMoving();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _playerMoveHandler.StopMoving();
        }
    }
}