using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class LevelCreator
    {
        private readonly Tilemap _obstacleTileMap;                  // Получать через DI 
        private readonly Tile[] _obstacleTiles;                     // Получать через DI ?
        private readonly ILevelConfig _level;                       // Получать через DI ?

        private readonly GameState _gameState;
        private readonly EntitiesFactory _entitiesFactory;          // Получать через DI

        private readonly bool _isLoaded = false;

        private Vector2 _fruitSpawnPosition;

        public LevelCreator(DIContainer sceneContainer, ILevelConfig levelConfig)
        {
            _obstacleTileMap = sceneContainer.Resolve<Tilemap>(GameConstants.Obstacle);
            _obstacleTiles = LoadTiles(GameConstants.WallTilesFolderPath, GameConstants.NumberOfWallTiles);
            var gameStateService = sceneContainer.Resolve<IGameStateService>();
            _level = levelConfig;
            _gameState = gameStateService.GameState;
            _entitiesFactory = _gameState.EntitiesFactory;                                  // Получать через DI
            _isLoaded = gameStateService.GameStateIsLoaded;

            ConstructLevel();

            var mapHandler = new MapHandlerService(_gameState, _obstacleTileMap, _fruitSpawnPosition);
            sceneContainer.RegisterInstance(mapHandler);   // Создание классов вынести в DI?
        }

        private void ConstructLevel()               // Передовать команды в MapHendler, чтобы только он менял Tilemap?
        {
            _obstacleTileMap.ClearAllTiles();
            var map = _level.Map;                   // Закешировать map, для предотвращения множественных копирований.

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    var cellPosition = new Vector3Int(x, -y);
                    int numTile = map[y, x];
                    numTile = numTile > 0 ? numTile - 1 : numTile + 1;

                    if (numTile >= 0)                                                              // Magic
                        CreateObstacle(numTile, x, y);
                    else if (numTile == GameConstants.FruitSpawn)
                        _fruitSpawnPosition = new Vector2(x, -y);
                    else if (_isLoaded == false)
                        CreateEntity(x, y, (EntityType)numTile);
                }
            }
        }

        private Tile[] LoadTiles(string folderPath, int count)
        {
            Tile[] tiles = new Tile[count];

            for (int tileName = 0; tileName < count; tileName++)
                tiles[tileName] = Resources.Load<Tile>($"{folderPath}{tileName}");

            return tiles;
        }

        private void CreateObstacle(int numTile, int x, int y)
        {
            var tilePos = new Vector3Int(x, -y);
            var tile = numTile > 0 ? _obstacleTiles[numTile] : null;
            _obstacleTileMap.SetTile(tilePos, tile);
        }

        private void CreateEntity(int x, int y, EntityType entityType)
        {
            //var entity = _gameState.Map.Value.Entities.FirstOrDefault(entity => entity.Type == entityType);

            //if (entity == null)
            //{
            var entity = _entitiesFactory.CreateEntity(new Vector2(x, y), entityType);
            _gameState.Map.Value.Entities.Add(entity);
            //}
        }
    }
}
