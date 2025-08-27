using System;
using System.Collections;
using UnityEngine;

namespace MyPacman
{
    public class ScoringService
    {
        private readonly GameState _gameState;
        private readonly IUIGameplayViewModel _uiGameplay;

        private int _scoreForRound;
        // For test
        private Coroutine _lifeUpShowing;

        // Массив подобранных фруктов
        // 1. Полноценное сохранение и загрузка данного массива
        // 2. Автоподписки на создание\удаление значков на панели согласно содержимому массива
        // 3. Автоподписка на создание\удаление значков на панели согласно кол-ву жизней игрока


        public event Action<int, Vector2> PointsReceived;            // для подписи сервиса который будет создавать\уничтожать view c сообщениями на экране

        public ScoringService(GameState gameState, MapHandlerService mapHandlerService, IUIGameplayViewModel uiGameplay)
        {
            _gameState = gameState;
            mapHandlerService.EntityEaten += OnEntityEaten;
            _uiGameplay = uiGameplay;
        }

        private void OnEntityEaten(EdibleEntityPoints enumPoints, Vector2 position)
        {
            int points = (int)enumPoints;
            _gameState.Score.Value += points;
            _scoreForRound += points;
            PointsReceived?.Invoke(points, position);

            CheckTheRequiredConditions();

            //For test
            if (_lifeUpShowing == null)
                _lifeUpShowing = Coroutines.StartRoutine(LifeUpFlickering(5f));
        }

        private void CheckTheRequiredConditions()
        {
            if (_scoreForRound >= GameConstants.PriceLifePoint)
                _gameState.LifePoints.Value++;
        }

        private IEnumerator LifeUpFlickering(float duration)           // Вынести логику в GameplayUIManager ??
        {
            // Проблема в том что данный скрипт будет срабатывать не только 
            // когда кол-во жизней увеличится, но и когда уменьшится
            float timer = 0f;
            float timeDelay = 0.4f;                                                                 // Magic
            var delay = new WaitForSeconds(timeDelay);
            string text = _uiGameplay.LifeUpText.text;

            while (timer < duration)
            {
                yield return delay;
                _uiGameplay.LifeUpText.text = string.Empty;

                yield return delay;
                _uiGameplay.LifeUpText.text = text;
                timer += (timeDelay * 2);
            }

            _lifeUpShowing = null;
        }

        //// For test
        //private void LoadAndShowFruits()
        //{
        //    var fruits = Resources.LoadAll<GameObject>("Prefabs/Fruits/Icons/");

        //    //StartCoroutine(PanelRecicle(fruits));
        //}

        ////For test     Создание и удаление фруктов в цикле
        //private IEnumerator PanelRecicle(GameObject[] fruits)
        //{
        //    float delay = 1f;
        //    int counter = 0;
        //    List<GameObject> fruitsLeftPanel = new();
        //    List<GameObject> fruitsRightPanel = new();
        //    bool fill = true;

        //    while (true)
        //    {
        //        if (fill)
        //        {
        //            if (counter < fruits.Length)
        //            {
        //                fruitsRightPanel.Add(CreateFruit(fruits[counter], _panelOfRecentlyPickedFruits));
        //                fruitsLeftPanel.Add(CreateFruit(fruits[counter], _lifeDisplayPanel));
        //                counter++;
        //            }
        //            else
        //            {
        //                fill = false;
        //            }
        //        }
        //        else
        //        {
        //            if (counter > 0)
        //            {
        //                // Удаление фруктов с наяала
        //                Destroy(fruitsLeftPanel[0]);
        //                fruitsLeftPanel.RemoveAt(0);

        //                counter--;
        //                // Удаление фруктов с конца
        //                Destroy(fruitsRightPanel[counter]);
        //                fruitsRightPanel.RemoveAt(counter);
        //            }
        //            else
        //            {
        //                fill = true;
        //            }
        //        }

        //        yield return new WaitForSeconds(delay);
        //    }
        //}

        //private GameObject CreateFruit(GameObject prefab, Transform parent)
        //{
        //    var fruit = Instantiate(prefab, parent);
        //    fruit.SetActive(true);
        //    return fruit;
        //}
    }
}