using UnityEngine;
using UnityEngine.InputSystem;

namespace MyPacman
{
    public class Pacman : MonoBehaviour
    {
        private int _pelletLayer;

        private PlayerInputActions _inputActions;
        private IPlayerMovementHandler _playerMoveHandler;
        private IMapHandler _mapHandler;

        private void Awake()
        {
            _pelletLayer = LayerMask.NameToLayer(GameConstants.Pellet);
        }

        private void OnEnable()
        {
            _playerMoveHandler = new PlayerMovementHandler(transform.GetComponent<Rigidbody2D>());
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.gameObject.name)
            {
                case GameConstants.TriggerPelletSmall:
                    Debug.Log(GameConstants.TriggerPelletSmall);
                    break;

                case GameConstants.TriggerPelletMedium:
                    Debug.Log(GameConstants.TriggerPelletMedium);
                    break;

                case GameConstants.TriggerPelletLarge:
                    Debug.Log(GameConstants.TriggerPelletLarge);
                    break;
            }

            HandleCollision(transform.position);
            //collision.gameObject.SetActive(false);
        }

        public void Initialize(IMapHandler mapHandler)
        {
            _mapHandler = mapHandler;
            //_playerMoveHandler.Initialyze(_mapHandler.IsObstacleTile);
            _playerMoveHandler.Initialyze(
                () => _inputActions.Keyboard.Movement.ReadValue<Vector2>(),
                _mapHandler.IsObstacleTile);
        }

        private void HandleCollision(Vector3 position)
        {
            _mapHandler.ChangeTile(position, GameConstants.EmptyTile);
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