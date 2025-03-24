using Game.Gameplay;
using Game.MainMenu;
using Game.Utils;
using Game.UI;
using R3;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DI;
using Game.Services;
using Game.State.Root;
using Game.State;

namespace Game
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;
        private readonly Coroutines _coroutines;
        private readonly DIContainer _rootContainer = new();    // Сюда ложим все что используется во всем проекте

        private DIContainer _cacheSceneContainer;
        private UIRootView _uiRootView;

        // Загружается до загрузки сцены
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutoStartGame()
        {
            // Если эта загрузка при запуске игры, то здесь нужно производить операции,
            // применение результатов которых требует перезапуска игры, чтобы не делать дополнительный перезапуск игры
            Application.targetFrameRate = 60;               // Устанавливаем ограничение FPS
            Screen.sleepTimeout = SleepTimeout.NeverSleep;  // Чтобы экран мобилки не гас если вы ничего не делаете (по умолчанию это отключенно)

            _instance = new GameEntryPoint();
            _instance.StartGame();
        }

        private GameEntryPoint()
        {
            _coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines.gameObject);

            var prefabUIRoot = Resources.Load<UIRootView>("Prefabs/UIRoot");
            _uiRootView = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(_uiRootView.gameObject);

            // Регистрируем корневую UI
            _rootContainer.RegisterInstance(_uiRootView);

            // Создаем систему загрузки/сохранения/сброса, и регестрируем ёё
            var gameStateProvider = new PlayerPrefsGameStateProvider();
            _rootContainer.RegisterInstance<IGameStateProvider>(gameStateProvider);

            // Регистрируем фабрику создания сервиса уровня проекта, создастся при первом запросе и будет существовать до конца игры
            _rootContainer.RegisterFactory(_ => new SomeCommonService()).AsSingle();
        }

        private void StartGame()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == ConstantsSceneNames.Gameplay)
            {
                // Заглушка для запуска уровня из редактора
                var gameplayEnterParams = new GameplayEnterParams("null.save", 1);
                _coroutines.StartCoroutine(LoadAndStartGameplay(gameplayEnterParams));
                return;
            }

            if (sceneName == ConstantsSceneNames.MainMenu)
            {
                _coroutines.StartCoroutine(LoadAndStartMainMenu());
                return;
            }

            if (sceneName != ConstantsSceneNames.Boot)
                return;
#endif
            _coroutines.StartCoroutine(LoadAndStartMainMenu());
        }

        private IEnumerator LoadAndStartGameplay(GameplayEnterParams gameplayEnterParams)
        {
            _uiRootView.ShowLoadingScreen();
            _cacheSceneContainer?.Dispose();    // Очищаем контейнер сцены, если он есть

            yield return LoadScene(ConstantsSceneNames.Boot);
            // Загрузку новой сцены делаем через промежуточную пустую, чтобы старая успела полностью выгрузится
            yield return LoadScene(ConstantsSceneNames.Gameplay);
            // Заглушка для продления загрузки сцены на 3 секунды
            // Необходима задержка как минимум на 1 кадр, так как GameplayEntryPoint создастся только в следующем кадре
            yield return new WaitForSeconds(1f);        // Заглушка ++++++++++++++++++++++++++++

            // Загрузка сотояния сцены(загрузка ранее сохраненной игры)
            bool isGameStateLoaded = false;
            // Достаем из контейнера провайдер и вызываем у него метод загрузки, а на его завершение подписываем лямбду меняющу переменную isGameStateLoaded
            _rootContainer.Resolve<IGameStateProvider>().LoadGameState().Subscribe(_ => isGameStateLoaded = true);
            // Ждем пока переменная isGameStateLoaded не изменится на true, тоесть пока загрузка не завершится
            yield return new WaitUntil(() => isGameStateLoaded);

            var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
            // Создаем новый контейнер для сцены и кэшируем его
            var gameplayContainer = _cacheSceneContainer = new DIContainer(_rootContainer);
            sceneEntryPoint.Run(gameplayContainer, gameplayEnterParams).Subscribe(gameplayExitParams =>
            {
                _coroutines.StartCoroutine(LoadAndStartMainMenu(gameplayExitParams.MainMenuEnterParams));
            });             // Здесь в загруженную сцену передаются данные

            _uiRootView.HideLoadingScreen();
        }

        // Можно сделать пораметры по умолчанию и передовать их при первом запуске игры вместо null
        private IEnumerator LoadAndStartMainMenu(MainMenuEnterParams mainMenuEnterParams = null)
        {
            _uiRootView.ShowLoadingScreen();
            _cacheSceneContainer?.Dispose();    // Очищаем контейнер сцены, если он есть

            yield return LoadScene(ConstantsSceneNames.Boot);
            // Загрузку новой сцены делаем через промежуточную пустую, чтобы старая успела полностью выгрузится
            yield return LoadScene(ConstantsSceneNames.MainMenu);
            // Заглушка для продления загрузки сцены на 3 секунды
            // Необходима задержка как минимум на 1 кадр, так как GameplayEntryPoint создастся только в следующем кадре
            yield return new WaitForSeconds(1f);        // Заглушка ++++++++++++++++++++++++++++

            var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            var mainMenuContainer = _cacheSceneContainer = new DIContainer(_rootContainer);
            // Здесь в загруженную сцену передаются данные, а на вернувшийся сигнал подписываем лямбду ?
            sceneEntryPoint.Run(mainMenuContainer, mainMenuEnterParams).Subscribe(mainMenuExitParams =>
            {
                var targetSceneName = mainMenuExitParams.TargetSceneEnterParams.SceneName;

                if (targetSceneName == ConstantsSceneNames.Gameplay)
                {
                    _coroutines.StartCoroutine(LoadAndStartGameplay(mainMenuExitParams.TargetSceneEnterParams
                                                                    .As<GameplayEnterParams>()));
                }
            });

            _uiRootView.HideLoadingScreen();
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
