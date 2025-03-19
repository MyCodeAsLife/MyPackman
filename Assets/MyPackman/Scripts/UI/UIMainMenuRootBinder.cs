using System;
using UnityEngine;

namespace UI
{
    public class UIMainMenuRootBinder : MonoBehaviour
    {
        public event Action GoToGameplayButtonClicked;

        public void OnGoToGameplayButtonClick()
        {
            GoToGameplayButtonClicked?.Invoke();
        }
    }
}