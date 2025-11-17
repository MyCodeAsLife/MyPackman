using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyPacman
{
    public class GameplayInputActionsHandler
    {
        private readonly GameplayUIManager _uiManager;
        private readonly PlayerInputActions _inputActions;
        private readonly PacmanMovementService _playerMovement;
        private readonly TextPopupService _textPopupService;
        private readonly TimeService _timeService;

        private readonly ReadOnlyReactiveProperty<Vector2> _textSpawnPos;
        private ScorePopupTextViewModel _text;

        // New
        //private bool _playerContorEnable;

        public GameplayInputActionsHandler(
            GameplayUIManager uiManager,
            PlayerInputActions inputActions,
            PacmanMovementService playerMovement,
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

            StartInit();
        }

        ~GameplayInputActionsHandler()
        {
            _inputActions.Keyboard.Movement.started -= _playerMovement.OnMoveStarted;
            _inputActions.Keyboard.Movement.canceled -= _playerMovement.OnMoveCanceled;
            _inputActions.Keyboard.Escape.performed -= OnEscapePressed;
            _inputActions.Disable();
        }

        // Вынести стартовую активацию ???
        private void StartInit()
        {
            _inputActions.Enable();
            _inputActions.Keyboard.Escape.performed += OnStartGameplayEscapePressed;
            _text = _textPopupService.ShowPopupText("READY!", _textSpawnPos.CurrentValue);  // Форматирование выводимого текста
            _text.SetColor(Color.red);
            _timeService.StopTime();
            // New
            //_timeService.IsTimeRun.Skip(1).Subscribe(OnPauseGamePressed);
            _inputActions.Keyboard.Movement.started += _playerMovement.OnMoveStarted;
            _inputActions.Keyboard.Movement.canceled += _playerMovement.OnMoveCanceled;
        }

        private void OnStartGameplayEscapePressed(InputAction.CallbackContext context)
        {
            if (_text != null)
            {
                _textPopupService.HidePopupText(_text);
                _text = null;
                _inputActions.Keyboard.Escape.performed -= OnStartGameplayEscapePressed;
                _inputActions.Keyboard.Escape.performed += OnEscapePressed;
                _timeService.RunTime();
            }
        }

        private void OnEscapePressed(InputAction.CallbackContext context)
        {
            if (_uiManager.IsScreenOpen)
                _uiManager.CloseScreenPauseMenu();
            else
                _uiManager.OpenScreenPauseMenu();
        }

        //private void OnPauseGamePressed(bool isRunTime)
        //{
        //    if (isRunTime != _playerContorEnable)
        //        SwitchPlayerControl();
        //}

        //private void SwitchPlayerControl()
        //{
        //    if (_playerContorEnable)
        //    {
        //        _playerContorEnable = false;
        //        _inputActions.Keyboard.Movement.started -= _playerMovement.OnMoveStarted;
        //        _inputActions.Keyboard.Movement.canceled -= _playerMovement.OnMoveCanceled;
        //    }
        //    else
        //    {
        //        _playerContorEnable = true;
        //        _inputActions.Keyboard.Movement.started += _playerMovement.OnMoveStarted;
        //        _inputActions.Keyboard.Movement.canceled += _playerMovement.OnMoveCanceled;
        //    }
        //}
    }
}
