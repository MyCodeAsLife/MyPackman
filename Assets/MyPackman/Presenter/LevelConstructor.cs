using Assets.MyPackman.Settings;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.MyPackman.Presenter
{
    public class LevelConstructor
    {
        private Tilemap _tileMap;                      // Получать через DI ?
        private Tile[] _textures;                     // Получать через DI ?
        private Packman _player;                      // Получать через DI ?
        private MapModel _map;                        // Получать через DI ?

        public LevelConstructor(Tilemap tileMap, Tile[] textures, Tile[] sprites, MapModel map)
        {
            _player = Resources.Load<Packman>("pacman");

            _tileMap = tileMap;
            _textures = textures;
            _map = map;
            _tileMap.ClearAllTiles();

            ConstructLevel();
        }

        private void ConstructLevel()
        {
            for (int y = 0; y < _map.Map.GetLength(0); y++)
            {
                for (int x = 0; x < _map.Map.GetLength(1); x++)
                {
                    if (_map.Map[y, x] > GameConstants.Player)
                        _tileMap.SetTile(new Vector3Int(x, -y), _textures[_map.Map[y, x]]);
                    else if (_map.Map[y, x] == GameConstants.Player)
                    {
                        var player = Object.Instantiate(_player);
                        player.transform.position = new Vector3(x * GameConstants.GridCellSize, -(y * GameConstants.GridCellSize));
                        player.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
