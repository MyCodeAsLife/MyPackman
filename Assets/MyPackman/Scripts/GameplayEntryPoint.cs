using System;
using UI;
using UnityEngine;

namespace Assets.MyPackman.Scripts
{
    public class GameplayEntryPoint : MonoBehaviour     // Похожа на MainMenuEntryPoint
    {
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;

        public event Action GoToMainMenuSceneRequested;

        public void Run(UIRootView uiRoot)                                       // Заглушка
        {
            var uiScene = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiScene.gameObject);

            uiScene.GoToMainMenuButtonClicked += () => { GoToMainMenuSceneRequested?.Invoke(); };
        }
    }
}
