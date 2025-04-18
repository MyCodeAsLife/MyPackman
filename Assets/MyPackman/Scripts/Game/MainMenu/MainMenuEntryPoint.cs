using DI;
using Game.Gameplay;
using Game.MainMenu.Static;
using Game.Services;
using Game.UI;
using R3;
using UnityEngine;

namespace Game.MainMenu
{
    public class MainMenuEntryPoint : MonoBehaviour     // ������ �� GameplayEntryPoint
    {
        [SerializeField] private UIMainMenuRootBinder _mainMenuUIRootPrefab;
        //private DIContainer _mainMenuContainer;

        // ���������� ������ �������, �� ������� ����� ������ ������� �������� GameplayExitParams
        public Observable<MainMenuExitParams> Run(DIContainer mainMenuContainer, MainMenuEnterParams mainMenuEnterParams)
        {
            // �������� ��������� ��� ���� ����� ��������� � �������� ���
            //_mainMenuContainer = mainMenuContainer;
            // ���������� ����������� ���� ����������� ��� ������ ����� ��������, � ���������� �����
            MainMenuRegistrations.Register(mainMenuContainer, mainMenuEnterParams);
            // ����� ��������� ������ ��������� �����? ����� ����� ������ ������� ����� ������� ��������� ViewModel-�?
            var mainMenuViewModelContainer = new DIContainer(mainMenuContainer);    // ���� �� ��� ����������?
            MainMenuViewModelRegistrations.Register(mainMenuViewModelContainer);

            //for tests
            mainMenuViewModelContainer.Resolve<UIMainMenuRootViewModel>();
            mainMenuViewModelContainer.Resolve<SomeMainMenuService>();

            var uiScene = Instantiate(_mainMenuUIRootPrefab);
            mainMenuContainer.Resolve<UIRootView>().AttachSceneUI(uiScene.gameObject);

            var exitSignalSubj = new Subject<Unit>();
            uiScene.Bind(exitSignalSubj);

            // ����� ������ � ����������� �����������, ����� � ��������, ������ ��� ��������� � MainMenu ���������� �� ������
            Debug.Log($"Main Menu entry point: Run main menu scene. Results: {mainMenuEnterParams?.Result}");

            // ��������� ������� ��� �����, ���� ���(������ ������ � �������) �� ����� ������� ��������� �� ���������
            int mapId = 0;
            var targetSceneEnterParams = new GameplayEnterParams(mapId);
            // ������������ �������������� ������ � ������� � ������ �������� ������ MainMenu
            var mainMenuExitParams = new MainMenuExitParams(targetSceneEnterParams);
            // ������������ ������ � ������ ������� � �������� ����� ������ ��������� � ���������� ���
            var exitToGameplaySceneSignal = exitSignalSubj.Select(_ => mainMenuExitParams);
            return exitToGameplaySceneSignal;
        }
    }
}