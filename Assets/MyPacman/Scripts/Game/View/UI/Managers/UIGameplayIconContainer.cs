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
        private List<GameObject> _fruitIcons = new();
        private List<GameObject> _playerLifeIcons = new();

        private void OnEnable()
        {
            LoadPrefabIcons();
        }

        public void ShowPlayerLifeIcon()
        {
            if (_playerLifeIcons.Count < GameConstants.MaxNumberLifeIconOnPanel)
            {
                var prefab = _prefabIcons[EntityType.Pacman];
                var createdIcon = CreateIcon(prefab, _playerLifeIconContainer);
                _playerLifeIcons.Add(createdIcon);
            }
        }

        public void ShowFruitIcon(EntityType fruitType)
        {
            if (fruitType > EntityType.Cherry || fruitType < EntityType.Key)
                return;

            var prefab = _prefabIcons[fruitType];
            var createdIcon = CreateIcon(prefab, _fruitIconContainer);

            if (GameConstants.MaxNumberFruitIconOnPanel <= _fruitIcons.Count)
            {
                var icon = _fruitIcons[0];
                _fruitIcons.RemoveAt(0);
                Destroy(icon);
            }

            _fruitIcons.Add(createdIcon);
        }

        public void HideIcon(EntityType entityType)
        {
            GameObject icon = null;

            if (entityType == EntityType.Pacman)
            {
                if (_playerLifeIcons.Count > 0)
                {
                    icon = _playerLifeIcons[0];
                    _playerLifeIcons.RemoveAt(0);
                }
            }
            else
            {
                if (_fruitIcons.Count > 0)
                {
                    icon = _fruitIcons.Last();
                    _fruitIcons.Remove(icon);
                }
            }

            if (icon != null)
                Destroy(icon);
        }

        public void OnPlayerLifePointsChanged(int numberLifePoints)
        {
            int numberIcons = _playerLifeIcons.Count;
            int difference = numberLifePoints - numberIcons;

            if (numberIcons != numberLifePoints)
            {
                if (difference > 0)
                {
                    for (int i = 0; i < difference; i++)
                        ShowPlayerLifeIcon();
                }
                else if (difference < 0 && GameConstants.MaxNumberLifeIconOnPanel > numberLifePoints)
                {
                    for (int i = difference; i < 0; i++)
                        HideIcon(EntityType.Pacman);
                }
            }
        }

        private void LoadPrefabIcons()
        {
            var fruits = Resources.LoadAll<GameObject>(GameConstants.IconsFolderPath);

            foreach (var fruit in fruits)
            {
                for (int i = (int)EntityType.Pacman; i >= (int)EntityType.Key; i--)
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

        private GameObject CreateIcon(GameObject prefab, Transform parent)
        {
            var fruit = Instantiate(prefab, parent);
            fruit.SetActive(true);
            return fruit;
        }
    }
}
