using System;
using UnityEngine;

namespace UI
{
    public class UIGameplayRootBinder : MonoBehaviour
    {
        public event Action GoToMainMenuButtonClicked;

        public void OnGoToMainMenuButtonClick()
        {
            GoToMainMenuButtonClicked?.Invoke();
        }
    }
}