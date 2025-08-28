using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    public class UIGameplayIconContainer : MonoBehaviour
    {
        [SerializeField] private Transform _fruitIconContainer;
        [SerializeField] private Transform _playerLifeIconContainer;

        // New
        private Dictionary<EntityType, GameObject> _prefabIcons = new();
        private Dictionary<EntityType, GameObject> _fruitIcons = new();
        private Dictionary<EntityType, GameObject> _playerLifeIcons = new();

        private void OnEnable()
        {
            LoadPrefabIcons();
        }

        public void ShowIcon(EntityType entityType)
        {
            if (_prefabIcons.TryGetValue(entityType, out GameObject prefab))
            {
                var container = _fruitIconContainer;
                var cashIcons = _fruitIcons;
                int maxNumberIconOnPanel = GameConstants.MaxNumberFruitIconOnPanel;

                if (entityType == EntityType.Pacman)
                {
                    container = _playerLifeIconContainer;
                    cashIcons = _playerLifeIcons;
                    maxNumberIconOnPanel = GameConstants.MaxNumberLifeIconOnPanel;
                }

                var createdIcon = CreateIcon(prefab, container);

                if (maxNumberIconOnPanel <= cashIcons.Count)
                    cashIcons.Remove(cashIcons.Last().Key);

                cashIcons.Add(entityType, createdIcon);
            }
        }

        public void HideIcon(EntityType entityType)
        {
            GameObject icon = null;

            if (entityType == EntityType.Pacman)
            {
                if (_playerLifeIcons.TryGetValue(entityType, out icon))
                    _playerLifeIcons.Remove(entityType);
            }
            else
            {
                if (_fruitIcons.TryGetValue(entityType, out icon))
                    _fruitIcons.Remove(entityType);
            }

            if (icon != null)
                Destroy(icon);
        }

        private void LoadPrefabIcons()
        {
            var fruits = Resources.LoadAll<GameObject>(GameConstants.IconsFolderPath);

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

        public void OnPlayerLifePointsChanged(int lifePoints)
        {
            if (_playerLifeIcons.Count != lifePoints)
            {
                int count = lifePoints - _playerLifeIcons.Count;

                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                        ShowIcon(EntityType.Pacman);

                    Debug.Log("Tut");                                       //+++++++++++++++++++++++++
                }
                else
                {
                    for (int i = count; i > 0; i++)
                        HideIcon(EntityType.Pacman);
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
