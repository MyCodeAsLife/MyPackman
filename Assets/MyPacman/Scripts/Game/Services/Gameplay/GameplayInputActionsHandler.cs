using UnityEngine.InputSystem;

namespace MyPacman
{
    public class GameplayInputActionsHandler
    {
        private readonly GameplayUIManager _uiManager;
        private readonly PlayerInputActions _inputActions;
        private readonly PlayerMovementService _playerMovement;

        public GameplayInputActionsHandler(
            GameplayUIManager uiManager,
            PlayerInputActions inputActions,
            PlayerMovementService playerMovement
            )
        {
            _uiManager = uiManager;
            _inputActions = inputActions;
            _playerMovement = playerMovement;

            _inputActions.Enable();
            _inputActions.Keyboard.Movement.started += _playerMovement.OnMoveStarted;
            _inputActions.Keyboard.Movement.canceled += _playerMovement.OnMoveCanceled;
            _inputActions.Keyboard.Escape.performed += OnEscapePressed;
        }

        ~GameplayInputActionsHandler()
        {
            _inputActions.Keyboard.Movement.started -= _playerMovement.OnMoveStarted;
            _inputActions.Keyboard.Movement.canceled -= _playerMovement.OnMoveCanceled;
            _inputActions.Keyboard.Escape.performed -= OnEscapePressed;
            _inputActions.Disable();
        }

        private void OnEscapePressed(InputAction.CallbackContext context)
        {
            if (_uiManager.IsScreenOpen)
                _uiManager.CloseScreenPauseMenu();
            else
                _uiManager.OpenScreenPauseMenu();
        }
    }
}
