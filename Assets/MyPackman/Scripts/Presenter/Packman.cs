using Assets.MyPackman.Model;
using Assets.MyPackman.Presenter;
using Assets.MyPackman.Settings;
using UnityEngine;
using UnityEngine.InputSystem;

public class Packman : MonoBehaviour
{
    private PlayerInputActions _inputActions;                       // Все сопутствующее вынести в класс PlayerInputController
    private IPlayerMovementHandler _playerMoveHandler;                                                          // DI - ? через interface

    private void OnEnable()
    {
        _playerMoveHandler = new PlayerMovementHandler(transform, FindFirstObjectByType<LevelPresenter>().MapHandler, this);        // Создание классов вынести в DI?
        _inputActions = new PlayerInputActions();                                                                                   // Создание классов вынести в DI?
        _inputActions.Enable();
        _inputActions.Keyboard.MoveLeft.started += OnMoveLeftStarted;
        _inputActions.Keyboard.MoveLeft.canceled += OnMoveLeftCanceled;
        _inputActions.Keyboard.MoveRight.started += OnMoveRightStarted;
        _inputActions.Keyboard.MoveRight.canceled += OnMoveRightCanceled;
        _inputActions.Keyboard.MoveUp.started += OnMoveUpStarted;
        _inputActions.Keyboard.MoveUp.canceled += OnMoveUpCanceled;
        _inputActions.Keyboard.MoveDown.started += OnMoveDownStarted;
        _inputActions.Keyboard.MoveDown.canceled += OnMoveDownCanceled;
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Keyboard.MoveLeft.started -= OnMoveLeftStarted;
        _inputActions.Keyboard.MoveLeft.canceled -= OnMoveLeftCanceled;
        _inputActions.Keyboard.MoveRight.started -= OnMoveRightStarted;
        _inputActions.Keyboard.MoveRight.canceled -= OnMoveRightCanceled;
        _inputActions.Keyboard.MoveUp.started -= OnMoveUpStarted;
        _inputActions.Keyboard.MoveUp.canceled -= OnMoveUpCanceled;
        _inputActions.Keyboard.MoveDown.started -= OnMoveDownStarted;
        _inputActions.Keyboard.MoveDown.canceled -= OnMoveDownCanceled;
    }

    private void Update()
    {
        _playerMoveHandler.Tick();
    }

    private void OnMoveDownStarted(InputAction.CallbackContext context)
    {
        _playerMoveHandler.SetMovementDirection(ConstantsGame.DownDirection);
    }

    private void OnMoveUpStarted(InputAction.CallbackContext context)
    {
        _playerMoveHandler.SetMovementDirection(ConstantsGame.UpDirection);
    }

    private void OnMoveRightStarted(InputAction.CallbackContext context)
    {
        _playerMoveHandler.SetMovementDirection(ConstantsGame.RightDirection);
    }

    private void OnMoveLeftStarted(InputAction.CallbackContext context)
    {
        _playerMoveHandler.SetMovementDirection(ConstantsGame.LeftDirection);
    }

    private void OnMoveDownCanceled(InputAction.CallbackContext context)
    {
        _playerMoveHandler.RemoveMovementDirection(ConstantsGame.DownDirection);
    }

    private void OnMoveUpCanceled(InputAction.CallbackContext context)
    {
        _playerMoveHandler.RemoveMovementDirection(ConstantsGame.UpDirection);
    }

    private void OnMoveRightCanceled(InputAction.CallbackContext context)
    {
        _playerMoveHandler.RemoveMovementDirection(ConstantsGame.RightDirection);
    }

    private void OnMoveLeftCanceled(InputAction.CallbackContext context)
    {
        _playerMoveHandler.RemoveMovementDirection(ConstantsGame.LeftDirection);
    }
}
