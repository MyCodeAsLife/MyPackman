using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    public class UIGameplayIconContainer : MonoBehaviour
    {
        [SerializeField] private Transform _fruitIconContainer;
        [SerializeField] private Transform _playerLifeIconContainer;

        // New
        private Dictionary<EntityType, GameObject> _prefabIcons = new();
        private List<GameObject> _fruitIcons = new();
        private List<GameObject> _playerLifeIcons = new();

        private void OnEnable()
        {
            LoadPrefabIcons();
        }

        private void LoadPrefabIcons()
        {
            var fruits = Resources.LoadAll<GameObject>("Prefabs/Fruits/Icons/");                // Magic

            foreach (var fruit in fruits)
            {
                for (int i = (int)EntityType.Pacman; i > (int)EntityType.Fruit; i--)
                {
                    EntityType iconType = (EntityType)i;
                    string iconName = iconType.ToString();

                    if (iconName == fruit.name)
                    {
                        _prefabIcons.Add(iconType, fruit);
                    }
                }
            }
        }

        // For test
        public void ShowFruits()
        {
            StartCoroutine(PanelRecicle());
        }

        //For test     Создание и удаление фруктов в цикле
        private IEnumerator PanelRecicle()
        {
            float delay = 1f;
            int counter = 0;
            //List<GameObject> _playerLifeIcons = new();
            //List<GameObject> _fruitIcons = new();
            bool fill = true;

            while (true)
            {
                if (fill)
                {
                    for (int i = (int)EntityType.Pacman; i > (int)EntityType.Fruit; i--)
                    {
                        if (_prefabIcons.TryGetValue((EntityType)i, out GameObject prefab))
                        {
                            if (counter < _prefabIcons.Count)
                            {
                                _fruitIcons.Add(CreateIcon(prefab, _fruitIconContainer));
                                _playerLifeIcons.Add(CreateIcon(prefab, _playerLifeIconContainer));
                                counter++;
                                yield return new WaitForSeconds(delay);
                            }
                            else
                            {
                                fill = false;
                            }
                        }
                    }
                }
                else
                {
                    if (counter > 0)
                    {
                        // Удаление фруктов с начала
                        Destroy(_playerLifeIcons[0]);
                        _playerLifeIcons.RemoveAt(0);

                        counter--;
                        // Удаление фруктов с конца
                        Destroy(_fruitIcons[counter]);
                        _fruitIcons.RemoveAt(counter);
                    }
                    else
                    {
                        fill = true;
                    }

                    yield return new WaitForSeconds(delay);
                }
            }
        }

        private GameObject CreateIcon(GameObject prefab, Transform parent)
        {
            var fruit = Instantiate(prefab, parent);
            fruit.SetActive(true);
            return fruit;
        }
    }
}
