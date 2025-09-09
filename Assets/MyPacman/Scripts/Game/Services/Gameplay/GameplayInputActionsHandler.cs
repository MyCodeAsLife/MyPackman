using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyPacman
{
    public class GameplayInputActionsHandler
    {
        private readonly GameplayUIManager _uiManager;
        private readonly PlayerInputActions _inputActions;
        private readonly PlayerMovementService _playerMovement;
        private readonly TextPopupService _textPopupService;
        private readonly TimeService _timeService;

        private ScorePopupTextViewModel _text;
        private ReadOnlyReactiveProperty<Vector2> _textSpawnPos;

        public GameplayInputActionsHandler(
            GameplayUIManager uiManager,
            PlayerInputActions inputActions,
            PlayerMovementService playerMovement,
            TextPopupService textPopupService,
            TimeService timeService,
            ReadOnlyReactiveProperty<Vector2> textSpawnPos
            )
        {
            _uiManager = uiManager;
            _inputActions = inputActions;
            _playerMovement = playerMovement;
            _textPopupService = textPopupService;
            _timeService = timeService;
            _textSpawnPos = textSpawnPos;

            //_inputActions.Enable();
            //_inputActions.Keyboard.Movement.started += _playerMovement.OnMoveStarted;
            //_inputActions.Keyboard.Movement.canceled += _playerMovement.OnMoveCanceled;
            //_inputActions.Keyboard.Escape.performed += OnEscapePressed;
            StartInit();
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

        // Вынести стартовую активацию ???
        private void StartInit()
        {
            _inputActions.Enable();
            _inputActions.Keyboard.Escape.performed += OnStartEscapePressed;
            _text = _textPopupService.ShowPopupText("READY!", _textSpawnPos.CurrentValue);  // Форматирование выводимого текста
            _text.SetColor(Color.red);
            _timeService.StopTime();
        }

        private void OnStartEscapePressed(InputAction.CallbackContext context)
        {
            if (_text != null)
            {
                _textPopupService.HidePopupText(_text);
                _text = null;
                _inputActions.Keyboard.Escape.performed -= OnStartEscapePressed;
                _inputActions.Keyboard.Escape.performed += OnEscapePressed;
                _inputActions.Keyboard.Movement.started += _playerMovement.OnMoveStarted;
                _inputActions.Keyboard.Movement.canceled += _playerMovement.OnMoveCanceled;
                _timeService.RunTime();
            }
        }
    }
}
