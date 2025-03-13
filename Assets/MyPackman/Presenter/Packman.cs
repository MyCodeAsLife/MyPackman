using Assets.MyPackman.Presenter;
using Assets.MyPackman.Settings;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Packman : MonoBehaviour
{
    private readonly Vector3[] DirectionOffset = new Vector3[4] { new Vector3(-GameSettings.Step, 0f, 0f), new Vector3(GameSettings.Step, 0f, 0f),
                                                                  new Vector3(0f, GameSettings.Step, 0f), new Vector3(0f, -GameSettings.Step, 0f) };   // Вынести в настройки??
    private readonly LevelMap _map = new LevelMap();
    private Coroutine _movement;
    private Transform _transform;
    private PlayerInputActions _inputActions;
    private bool[] _directionsPresed = new bool[4];
    private int _lastDirection = GameSettings.NoDirection;
    private LevelConstructor _constructor;                                                                  // для тестов

    private event Action Moved;

    private void OnEnable()
    {
        _transform = GetComponent<Transform>();
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
        _inputActions.Keyboard.MoveLeft.started += OnMoveLeftStarted;
        _inputActions.Keyboard.MoveLeft.canceled += OnMoveLeftCanceled;
        _inputActions.Keyboard.MoveRight.started += OnMoveRightStarted;
        _inputActions.Keyboard.MoveRight.canceled += OnMoveRightCanceled;
        _inputActions.Keyboard.MoveUp.started += OnMoveUpStarted;
        _inputActions.Keyboard.MoveUp.canceled += OnMoveUpCanceled;
        _inputActions.Keyboard.MoveDown.started += OnMoveDownStarted;
        _inputActions.Keyboard.MoveDown.canceled += OnMoveDownCanceled;

        _constructor = FindFirstObjectByType<LevelConstructor>();                                           // для тестов
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
        Moved?.Invoke();
    }

    private void OnMoveDownStarted(InputAction.CallbackContext context)
    {
        SetDirection(GameSettings.DownDirection);
    }

    private void OnMoveUpStarted(InputAction.CallbackContext context)
    {
        SetDirection(GameSettings.UpDirection);
    }

    private void OnMoveRightStarted(InputAction.CallbackContext context)
    {
        SetDirection(GameSettings.RightDirection);
    }

    private void OnMoveLeftStarted(InputAction.CallbackContext context)
    {
        SetDirection(GameSettings.LeftDirection);
    }

    private void OnMoveDownCanceled(InputAction.CallbackContext context)
    {
        RemoveDirection(GameSettings.DownDirection);
    }

    private void OnMoveUpCanceled(InputAction.CallbackContext context)
    {
        RemoveDirection(GameSettings.UpDirection);
    }

    private void OnMoveRightCanceled(InputAction.CallbackContext context)
    {
        RemoveDirection(GameSettings.RightDirection);
    }

    private void OnMoveLeftCanceled(InputAction.CallbackContext context)
    {
        RemoveDirection(GameSettings.LeftDirection);
    }

    private void SetDirection(int currentDirectionPresed)
    {
        _directionsPresed[currentDirectionPresed] = true;

        for (int i = 0; i < _directionsPresed.Length; i++)
            if (_directionsPresed[i] && i != currentDirectionPresed)
                return;

        Moved += Move;
    }

    private void RemoveDirection(int removedDirection)
    {
        _directionsPresed[removedDirection] = false;

        for (int i = 0; i < _directionsPresed.Length; i++)
            if (_directionsPresed[i])
                return;

        Moved -= Move;
        _lastDirection = GameSettings.NoDirection;
    }

    private void Move()
    {
        if (_movement == null)
        {
            int currentDirection = GameSettings.NoDirection;

            for (int i = 0; i < _directionsPresed.Length; i++)
                if (_directionsPresed[i])
                    currentDirection = i;

            if (_lastDirection != GameSettings.NoDirection)
                for (int i = 0; i < _directionsPresed.Length; i++)
                    if (_directionsPresed[i] && i != _lastDirection)
                        currentDirection = i;

            var nextPosition = _transform.position + DirectionOffset[currentDirection];
            _lastDirection = currentDirection;

            if (IsAvalableCell(nextPosition, currentDirection))
            {
                _movement = StartCoroutine(Moving(nextPosition));
            }
        }
    }

    private IEnumerator Moving(Vector3 nextPosition)
    {
        while (_transform.position != nextPosition)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, nextPosition, GameSettings.PlayerSpeed);
            yield return null;
        }

        _movement = null;
    }

    private bool IsAvalableCell(Vector3 nexpPosition, int direction)
    {
        int x = (int)(Mathf.Ceil(nexpPosition.x * 10));
        int y = (int)(Mathf.Ceil(nexpPosition.y * 10));

        Debug.Log($"{x} {y}");                                                                    //++++++++++++++++++++++++++++++++

        if (x % 2 != 0 || y % 2 != 0)
        {
            nexpPosition = nexpPosition + DirectionOffset[direction];
            //Debug.Log(nexpPosition);                                                                    //++++++++++++++++++++++++++++++++
        }


        if (GetMapTile(nexpPosition) == 0)
            return false;

        return true;
    }

    private int GetMapTile(Vector3 position)
    {
        int x = (int)(Mathf.Round(position.x * 10)) / 2;
        int y = (int)(Mathf.Round(position.y * 10)) / 2;
        //var x = (int)(position.x / GameSettings.GridCellSize);
        //var y = (int)(position.y / GameSettings.GridCellSize);
        Debug.Log(position);                                                                    //++++++++++++++++++++++++++++++++
        Debug.Log($"{x} {y}");                                                                  //+++++++++++++++++++++++++++++++
        Vector3Int pos = new Vector3Int(x, y);                                                  //+++++++++++++++++++++++++++++++
        _constructor.AddTestObject(pos);                                                        //+++++++++++++++++++++++++++++++
        return _map.Map[-y, x];
    }
}
