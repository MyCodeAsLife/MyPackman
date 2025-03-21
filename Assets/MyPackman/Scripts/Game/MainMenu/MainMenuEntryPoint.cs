using DI;
using Game.Gameplay;
using Game.UI;
using R3;
using UnityEngine;

namespace Game.MainMenu
{
    public class MainMenuEntryPoint : MonoBehaviour     // ������ �� GameplayEntryPoint
    {
        [SerializeField] private UIMainMenuRootBinder _mainMenuUIRootPrefab;
        private DIContainer _container;

        // ���������� ������ �������, �� ������� ����� ������ ������� �������� GameplayExitParams
        public Observable<MainMenuExitParams> Run(DIContainer container, MainMenuEnterParams mainMenuEnterParams)
        {
            _container = container;
            var uiScene = Instantiate(_mainMenuUIRootPrefab);
            _container.Resolve<UIRootView>().AttachSceneUI(uiScene.gameObject);

            var exitSignalSubj = new Subject<Unit>();
            uiScene.Bind(exitSignalSubj);

            // ����� ������ � ����������� �����������, ����� � ��������, ������ ��� ��������� � MainMenu ���������� �� ������
            Debug.Log($"Main Menu entry point: Run main menu scene. Results: {mainMenuEnterParams?.Result}");

            // ��������� ������� ��� �����, ���� ���(������ ������ � �������) �� ����� ������� ��������� �� ���������
            var saveFileName = "Is null";
            int levelNumber = 2;
            var targetSceneEnterParams = new GameplayEnterParams(saveFileName, levelNumber);
            // ������������ �������������� ������ � ������� � ������ �������� ������ MainMenu
            var mainMenuExitParams = new MainMenuExitParams(targetSceneEnterParams);
            // ������������ ������ � ������ ������� � �������� ����� ������ ��������� � ���������� ���
            var exitToGameplaySceneSignal = exitSignalSubj.Select(_ => mainMenuExitParams);
            return exitToGameplaySceneSignal;
        }
    }
}