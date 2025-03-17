using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.MyPackman.Presenter
{
    class MapHandler
    {
        private LevelMap _map = new();           // DI - ?
        private Tilemap _tilemap;                // DI - ?
        private Tile[] _textures;                // DI - ?

        public int Tile(Vector3Int position) => _map.Map[-position.y, position.x];

        public void ChangeTile(Vector3Int position, int newTile)        // Через шину событий?
        {
            _map.Map[position.y, position.x] = newTile;                                         // Изменяет Модель
            var tile = newTile > 0 ? _textures[newTile] : new Tile();
            _tilemap.SetTile(new Vector3Int(position.x, -position.y), tile);                    // Изменяет Presenter
        }
    }
}
