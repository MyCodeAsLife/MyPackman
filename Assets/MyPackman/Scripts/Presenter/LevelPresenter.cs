using UnityEngine.Tilemaps;
using UnityEngine;
using Assets.MyPackman.Presenter;
using Assets.MyPackman.Settings;

namespace Assets.MyPackman.Model
{
    public class LevelPresenter : MonoBehaviour                     // LevelEntryPoint ????
    {
        [SerializeField] private Tilemap _tilemap;                      // Раздавать через DI ??
        [SerializeField] private Tile[] _textures;                      // Раздавать через DI ??
        [SerializeField] private Tile[] _sprites;                      // Раздавать через DI ??

        private MapModel _map;                              // Раздавать через DI ??
        private IMapHandler _mapHandler;                     // Раздавать через DI ??

        public IMapHandler MapHandler => _mapHandler;

        private void OnEnable()
        {
            _map = new MapModel();
            _mapHandler = new MapHandler(_tilemap, _textures, _map);                // Создание классов вынести в DI?
            new LevelConstructor(_tilemap, _textures, _sprites, _map);              // Создание классов вынести в DI?

            float y = _map.Map.GetLength(0) * ConstantsGame.GridCellSize * 0.5f;            // Magic
            float x = _map.Map.GetLength(1) * ConstantsGame.GridCellSize * 0.5f;            // Magic
            Camera.main.transform.position = new Vector3(x, -y, -10);            // Magic
            //Camera.main.orthographicSize = y + GameSettings.GridCellSize;      // Размер проекции камеры должен быть кратем размерам спрайтов дабы избежать искажений последних
        }
    }
}
