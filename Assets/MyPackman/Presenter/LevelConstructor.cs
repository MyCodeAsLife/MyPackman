using Assets.MyPackman.Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace Assets.MyPackman.Presenter
{
    class LevelConstructor : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tilemap _testTilemap;
        [SerializeField] private Tile[] _tiles;
        [SerializeField] private Packman _player;
        //[SerializeField] private Palette
        private bool _isTesting = false;                            //для тестов

        private LevelMap _map;

        private void OnEnable()
        {
            _map = new LevelMap();           // Откуда и как получать карты?
            _tilemap.ClearAllTiles();

            ConstructLevel();

            //Camera position по центру карты
            float y = _map.Map.GetLength(0) * 0.2f * 0.5f;
            float x = _map.Map.GetLength(1) * 0.2f * 0.5f;
            Camera.main.transform.position = new Vector3(x, -y, -10);
            Debug.Log(Camera.main.pixelHeight);                                             //+++++++++++++++++++++++++++++++++++
            Debug.Log(Camera.main.pixelWidth);                                             //+++++++++++++++++++++++++++++++++++

            Debug.Log(y + GameSettings.GridCellSize);       // Размер проекции камеры 
        }

        private void ConstructLevel()
        {
            for (int y = 0; y < _map.Map.GetLength(0); y++)
            {
                for (int x = 0; x < _map.Map.GetLength(1); x++)
                {
                    if (_map.Map[y, x] == 0)
                        _tilemap.SetTile(new Vector3Int(x, -y), _tiles[227]);
                    else if (_map.Map[y, x] == 3)
                        Instantiate(_player).transform.position = new Vector3(x * 0.2f, -(y * 0.2f));
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
            var timer = 1f;
            var delay = new WaitForSeconds(0.3f);

            while (timer > 0)
            {
                _testTilemap.SetTile(position, _tiles[93]);
                yield return delay;
                _testTilemap.ClearAllTiles();
                yield return delay;
                timer -= 0.6f;
            }

            _isTesting = false;
        }
    }
}
