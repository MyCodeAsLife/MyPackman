using UnityEngine;
using UnityEngine.InputSystem;

namespace MyPacman
{
    public class Pacman : MonoBehaviour
    {
        private int _pelletLayer;
        private Rigidbody2D _rigidbody;

        private PlayerInputActions _inputActions;
        private IPlayerMovementHandler _playerMoveHandler;
        private IMapHandler _mapHandler;

        private void Awake()
        {
            _pelletLayer = LayerMask.NameToLayer(GameConstants.Pellet);
        }

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerMoveHandler = new PlayerMovementHandler(_rigidbody);
            //_playerMoveHandler.Initialyze(() => _inputActions.Keyboard.Movement.ReadValue<Vector2>());

            _inputActions = new PlayerInputActions();
            _inputActions.Enable();
            _inputActions.Keyboard.Movement.started += OnMoveStarted;
            _inputActions.Keyboard.Movement.canceled += OnMoveCanceled;
        }

        private void OnDisable()
        {
            _inputActions.Disable();
            _inputActions.Keyboard.Movement.performed -= OnMoveStarted;
        }

        private void Update()
        {
            _playerMoveHandler.Tick();
        }

        public void Initialize(IMapHandler mapHandler)
        {
            _mapHandler = mapHandler;
            var mapSize = new Vector2(_mapHandler.Map.GetLength(1), -_mapHandler.Map.GetLength(0));

            _playerMoveHandler.Initialyze(
                () => _inputActions.Keyboard.Movement.ReadValue<Vector2>(),
                _mapHandler.IsObstacleTile,
                mapSize);

            _playerMoveHandler.TileChanged += _mapHandler.OnPlayerTilesChanged;         // Вынести в отдельный инициализатор
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