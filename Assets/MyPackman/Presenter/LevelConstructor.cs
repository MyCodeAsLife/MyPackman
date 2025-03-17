using Assets.MyPackman.Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.MyPackman.Presenter
{
    class LevelConstructor : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tilemap _testTilemap;
        [SerializeField] private Tile[] _textures;
        [SerializeField] private Tile[] _sprites;
        [SerializeField] private Packman _player;
        private bool _isTesting = false;                            //для тестов

        private LevelMap _map;

        private void OnEnable()
        {
            _map = new LevelMap();           // Откуда и как получать карты?
            _tilemap.ClearAllTiles();

            ConstructLevel();

            //Camera position по центру карты
            float y = _map.Map.GetLength(0) * GameSettings.GridCellSize * 0.5f;            // Magic
            float x = _map.Map.GetLength(1) * GameSettings.GridCellSize * 0.5f;            // Magic
            Camera.main.transform.position = new Vector3(x, -y, -10);            // Magic
            //Camera.main.orthographicSize = y + GameSettings.GridCellSize;      // Размер проекции камеры должен быть кратем размерам спрайтов дабы избежать искажений последних
        }

        private void ConstructLevel()
        {
            for (int y = 0; y < _map.Map.GetLength(0); y++)
            {
                for (int x = 0; x < _map.Map.GetLength(1); x++)
                {
                    if (_map.Map[y, x] > GameSettings.PackmanSpawn)
                        _tilemap.SetTile(new Vector3Int(x, -y), _textures[_map.Map[y, x]]);
                    else if (_map.Map[y, x] == GameSettings.PackmanSpawn)
                        Instantiate(_player).transform.position = new Vector3(x * GameSettings.GridCellSize, -(y * GameSettings.GridCellSize));
                }
            }
        }

        public void AddTestObject(Vector3Int position)                            //для тестов
        {
            if (_isTesting == false)
            {
                _isTesting = true;
                StartCoroutine(Testing(position));
            }
        }

        private IEnumerator Testing(Vector3Int position)                            //для тестов
        {
            var timer = 1f;            // Magic
            var delay = new WaitForSeconds(0.3f);            // Magic

            while (timer > 0)
            {
                _testTilemap.SetTile(position, _textures[93]);
                yield return delay;
                _testTilemap.ClearAllTiles();
                yield return delay;
                timer -= 0.6f;            // Magic
            }

            _isTesting = false;
        }
    }
}
