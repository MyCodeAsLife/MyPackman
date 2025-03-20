using System;
using Game.UI;
using UnityEngine;

namespace Assets.MyPackman.Scripts
{
    public class MainMenuEntryPoint : MonoBehaviour     // Похожа на GameplayEntryPoint
    {
        [SerializeField] private UIMainMenuRootBinder _mainMenuUIRootPrefab;

        public event Action GoToGameplaySceneRequested;

        public void Run(UIRootView uiRoot)                                       // Заглушка
        {
            var uiScene = Instantiate(_mainMenuUIRootPrefab);
            uiRoot.AttachSceneUI(uiScene.gameObject);

            uiScene.GoToGameplayButtonClicked += () =>
            {
                GoToGameplaySceneRequested?.Invoke();
            };
        }
    }
}