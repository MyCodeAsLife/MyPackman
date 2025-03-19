using System;
using UI;
using UnityEngine;

namespace Assets.MyPackman.Scripts
{
    public class MainMenuEntryPoint : MonoBehaviour     // ������ �� GameplayEntryPoint
    {
        [SerializeField] private UIMainMenuRootBinder _mainMenuUIRootPrefab;

        public event Action GoToGameplaySceneRequested;

        public void Run(UIRootView uiRoot)                                       // ��������
        {
            var uiScene = Instantiate(_mainMenuUIRootPrefab);
            uiRoot.AttachSceneUI(uiScene.gameObject);

            uiScene.GoToGameplayButtonClicked += () => { GoToGameplaySceneRequested?.Invoke(); };
        }
    }
}