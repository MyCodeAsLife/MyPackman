using R3;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyPacman
{
    public class AppEntryPoint
    {
        private static AppEntryPoint _instance;

        private readonly DIContainer _projectContainer = new();

        //private UIRootView _uiRoot;
        private DIContainer _cashedSceneContainer;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoStartApplication()
        {
            _instance = new AppEntryPoint();
            _instance.RunApplication();
        }

        private AppEntryPoint()
        {
            //var settingsProvider = new SettingsProvider();                              // Сервис настроек
            //_projectContainer.RegisterInstance<ISettingsProvider>(settingsProvider);    // Регистрация сервиса настроек
            //var gameStateProvider = new PlayerPrefsGameStateProvider();                 // Сервис загрузки\сохранения

            //_projectContainer.RegisterFactory(_ => new SomeCommonService()).AsSingle(); // Некий тестовый сервис
            //_projectContainer.RegisterInstance<IGameStateProvider>(gameStateProvider);  // Регистрация сервиса загрузки\сохранения

            //_projectContainer.Resolve<IGameStateProvider>().LoadSettingsState();        // Загрузка настроек из соранения

            //var loadingScreenPrefab = Resources.Load<UIRootView>(GameConstants.UIRootViewFullPath); // Загрузка префаба корневого UI
            //_uiRoot = Object.Instantiate(loadingScreenPrefab);                                      // Создание корневого UI из префаба
            //_projectContainer.RegisterInstance(_uiRoot);                                            // Регистрация корневого UI
        }

        private /*async*/ void RunApplication()
        {
            //await _projectContainer.Resolve<ISettingsProvider>().LoadBasicGameSettingsAsync();  // Асинхронная загрузка базовых настроек

#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == GameConstants.GameplayScene)
            {
                var defaultScene = new GameplayEnterParams("ddd.sv", 1);
                Coroutines.StartRoutine(LoadAndStartGameplay(defaultScene));
                return;
            }

            if (sceneName == GameConstants.MainMenuScene)
            {
                Coroutines.StartRoutine(LoadAndStartMainMenu());
                return;
            }

            if (sceneName != GameConstants.BootScene)
            {
                return;
            }
#endif

            Coroutines.StartRoutine(LoadAndStartMainMenu());
        }

        private IEnumerator LoadAndStartMainMenu(MainMenuEnterParams mainMenuEnterParams = null)
        {
            //_uiRoot.ShowLoadingScreen();
            _cashedSceneContainer?.Dispose();

            yield return LoadScene(GameConstants.BootScene);
            yield return LoadScene(GameConstants.MainMenuScene);

            yield return new WaitForSeconds(1f);

            var mainMenuContainer = _cashedSceneContainer = new DIContainer(_projectContainer);
            var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            sceneEntryPoint.Run(mainMenuEnterParams, mainMenuContainer).Subscribe(mainMenuExitParams =>
            {
                Coroutines.StartRoutine(LoadAndStartGameplay(mainMenuExitParams.TargetSceneEnterParams));
            });

            //_uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadAndStartGameplay(SceneEnterParams sceneEnterParams)
        {
            //_uiRoot.ShowLoadingScreen();
            _cashedSceneContainer?.Dispose();

            yield return LoadScene(GameConstants.BootScene);
            yield return LoadScene(GameConstants.GameplayScene);

            yield return new WaitForSeconds(1f);

            //bool isGameStateLoaded = false;
            //_projectContainer.Resolve<IGameStateProvider>().LoadGameState().Subscribe(_ => isGameStateLoaded = true);
            //yield return new WaitUntil(() => isGameStateLoaded);

            var gameplayContainer = _cashedSceneContainer = new DIContainer(_projectContainer);
            //var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
            var sceneEntryPoint = Object.FindFirstObjectByType<SceneEntryPoint>();

            sceneEntryPoint.Run(sceneEnterParams, gameplayContainer).Subscribe(gameplayExitParams =>
            {
                Coroutines.StartRoutine(LoadAndStartMainMenu(gameplayExitParams.MainMenuEnterParams));
            });

            //_uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}