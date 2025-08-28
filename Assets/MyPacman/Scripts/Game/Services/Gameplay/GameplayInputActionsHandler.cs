using UnityEngine;
using UnityEngine.InputSystem;

namespace MyPacman
{
    public class GameplayInputActionsHandler
    {
        private readonly GameplayUIManager _uiManager;
        private readonly PlayerInputActions _inputActions;
        private readonly PlayerMovementService _playerMovement;

        // For test
        private readonly GameState _gameState;
        private int _counter;

        public GameplayInputActionsHandler(
            GameplayUIManager uiManager,
            PlayerInputActions inputActions,
            PlayerMovementService playerMovement,
            GameState gameState                         // For test
            )
        {
            _uiManager = uiManager;
            _inputActions = inputActions;
            _playerMovement = playerMovement;
            _gameState = gameState;                     // For test

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
            //  НЕ УДАЛЯТЬ!! Закоментированно на время тестирования других функций
            //if (_uiManager.IsScreenOpen)
            //    _uiManager.CloseScreenPauseMenu();
            //else
            //    _uiManager.OpenScreenPauseMenu();
            //--------------------------------------------------------------------------
            // For test
            EntityType entity = EntityType.Chery - _counter;

            if (entity <= EntityType.Chery && entity > EntityType.Fruit)
            {
                _gameState.PickedFruits.Add(entity);
            }

            _counter++;

            if (Random.Range(0, 2) > 0)
                _gameState.LifePoints.Value++;
            else
                _gameState.LifePoints.Value--;
        }
    }
}
