using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.MyPackman.Presenter
{
    public class MapHandler : IMapHandler                        // Разделить на ModelMapHandler и PresenterMapHandler и связать через шину событий
    {
        private MapModel _map;                 // DI - ? через interface
        private Tilemap _tilemap;                // DI - ?
        private Tile[] _textures;                // DI - ?

        public MapHandler(Tilemap tilemap, Tile[] textures, MapModel map)
        {
            _tilemap = tilemap;
            _textures = textures;
            _map = map;
        }

        public int Tile(Vector3Int position) => _map.Map[position.y, position.x];

        public void ChangeTile(Vector3Int position, int objectNumber)        // Через шину событий?
        {
            _map.Map[position.y, position.x] = objectNumber;                                    // Изменяет Модель
            var tile = objectNumber > 0 ? _textures[objectNumber] : null;
            _tilemap.SetTile(new Vector3Int(position.x, -position.y), tile);                    // Изменяет Presenter
        }

        public bool TryFindPositionByObjectNumber(int number, ref Vector3Int position)
        {
            Vector3Int tileNumber = Vector3Int.zero;

            for (int y = 0; y < _map.Map.GetLength(0); y++)
            {
                for (int x = 0; x < _map.Map.GetLength(1); x++)
                {
                    if (_map.Map[y, x] == number)
                    {
                        position = new Vector3Int(x, y);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
