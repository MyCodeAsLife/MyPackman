using Assets.MyPackman.Scripts.Settings;
using Game.UI;
using R3;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MyPackman.Scripts
{
    public class GameEntryPoint
    {
        private readonly Coroutines _coroutines;
        private static GameEntryPoint _instance;
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
        }

        private void StartGame()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == ConstantsSceneNames.Gameplay)
            {
                _coroutines.StartCoroutine(LoadAndStartGameplay());
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

        private IEnumerator LoadAndStartGameplay(GameplayEnterParams gameplayEnterParams)      // Заглушка
        {
            _uiRootView.ShowLoadingScreen();

            yield return LoadScene(ConstantsSceneNames.Boot);
            // Загрузку новой сцены делаем через промежуточную пустую, чтобы старая успела полностью выгрузится
            yield return LoadScene(ConstantsSceneNames.Gameplay);
            // Заглушка для продления загрузки сцены на 3 секунды
            // Необходима задержка как минимум на 1 кадр, так как GameplayEntryPoint создастся только в следующем кадре
            yield return new WaitForSeconds(2f);

            var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
            sceneEntryPoint.Run(_uiRootView, gameplayEnterParams).Subscribe(gameplayExitParams =>
            {
                _coroutines.StartCoroutine(LoadAndStartMainMenu());
            });             // Здесь в загруженную сцену передаются данные

            _uiRootView.HideLoadingScreen();
        }

        private IEnumerator LoadAndStartMainMenu()      // Заглушка
        {
            _uiRootView.ShowLoadingScreen();

            yield return LoadScene(ConstantsSceneNames.Boot);
            // Загрузку новой сцены делаем через промежуточную пустую, чтобы старая успела полностью выгрузится
            yield return LoadScene(ConstantsSceneNames.MainMenu);
            // Заглушка для продления загрузки сцены на 3 секунды
            // Необходима задержка как минимум на 1 кадр, так как GameplayEntryPoint создастся только в следующем кадре
            yield return new WaitForSeconds(2f);

            var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            sceneEntryPoint.Run(_uiRootView);                                 // Здесь в загруженную сцену передаются данные

            // Грязный код!!!!!!!!!
            sceneEntryPoint.GoToGameplaySceneRequested += () =>
            {
                _coroutines.StartCoroutine(LoadAndStartGameplay());
            };

            _uiRootView.HideLoadingScreen();
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
