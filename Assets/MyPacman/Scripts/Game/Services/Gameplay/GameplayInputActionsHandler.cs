using UnityEngine.InputSystem;

namespace MyPacman
{
    public class GameplayInputActionsHandler
    {
        private readonly GameplayUIManager _uiManager;
        private readonly PlayerInputActions _inputActions;
        private readonly PlayerMovementService _playerMovement;

        // For test
        //private readonly GameState _gameState;
        //private int _number;
        //private bool _switcher = false;

        public GameplayInputActionsHandler(
            GameplayUIManager uiManager,
            PlayerInputActions inputActions,
            PlayerMovementService playerMovement
            //GameState gameState                         // For test
            )
        {
            _uiManager = uiManager;
            _inputActions = inputActions;
            _playerMovement = playerMovement;
            //_gameState = gameState;                     // For test

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
            //--------------------------------------------------------------------------
            // For test
            //TestIconsUI();
        }

        //private void TestIconsUI()
        //{
        //    //EntityType entity = EntityType.Cherry - _counter;

        //    int num = Random.Range((int)EntityType.Cherry, (int)EntityType.Fruit);
        //    EntityType entity = (EntityType)num;
        //        _gameState.PickedFruits.Add(entity);


        //    if (_switcher == false)
        //    {
        //        if(_gameState.LifePoints.Value > 13)
        //            _switcher = true;
        //    }
        //    else
        //    {
        //        if(_gameState.LifePoints.Value < 3)
        //            _switcher = false;
        //    }

        //    if (_switcher)
        //        _gameState.LifePoints.Value--;
        //    else
        //        _gameState.LifePoints.Value++;
        //}
    }
}
