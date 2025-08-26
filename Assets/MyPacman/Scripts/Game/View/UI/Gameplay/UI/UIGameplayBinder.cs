using R3;
using TMPro;
using UnityEngine;

namespace MyPacman
{
    public class UIGameplayBinder : WindowBinder<UIGameplayViewModel>
    {
        [SerializeField] private TextMeshProUGUI _highScore;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private GameObject _lifeUpText;                            // Вернуть во вьюмодель для для использования в функции мигания
        [SerializeField] private Transform _panelOfRecentlyPickedFruits;            // Вернуть во вьюмодель для добавления на нее иконок
        [SerializeField] private Transform _lifeDisplayPanel;                       // Вернуть во вьюмодель для добавления на нее иконок

        // Здесь передаем на UI изменения из GameState (очки, жизни, время и т.д.)
        protected override void OnBind()
        {
            ViewModel.HighScore.Subscribe(highScore => _highScore.text = highScore.ToString());
            ViewModel.Score.Subscribe(score => _score.text = score.ToString());
            //ViewModel.LifePoints.Skip(1).Subscribe(_ => Coroutines.StartRoutine(LifeUpShowing(4f)));        // Magic

            // New
            ViewModel.SetActiveLifeUpText = _lifeUpText.SetActive;
            ViewModel.PanelOfRecentlyPickedFruits = _panelOfRecentlyPickedFruits;
            ViewModel.LifeDisplayPanel = _lifeDisplayPanel;

            //// For test
            //LoadAndShowFruits();
        }

        //private IEnumerator LifeUpShowing(float duration)           // Вынести логику в GameplayUIManager ??
        //{
        //    // Проблема в том что данный скрипт будет срабатывать не только 
        //    // когда кол-во жизней увеличится, но и когда уменьшится
        //    float timer = 0f;
        //    float timeDelay = 0.7f;                                                                 // Magic
        //    var delay = new WaitForSeconds(timeDelay);

        //    while (timer < duration)
        //    {
        //        yield return delay;
        //        _lifeUpText.SetActive(false);
        //        yield return delay;
        //        _lifeUpText.SetActive(true);
        //        timer += Time.deltaTime;
        //    }
        //}

        //// For test
        //private void LoadAndShowFruits()
        //{
        //    var fruits = Resources.LoadAll<GameObject>("Prefabs/Fruits/Icons/");

        //    StartCoroutine(PanelRecicle(fruits));
        //}

        //// For test     Создание и удаление фруктов в цикле
        //IEnumerator PanelRecicle(GameObject[] fruits)
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
