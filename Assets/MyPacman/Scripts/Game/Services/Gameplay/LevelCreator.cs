using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class LevelCreator
    {
        private readonly Tilemap _obstacleTileMap;                  // Получать через DI 
        private readonly Tile[] _obstacleTiles;                     // Получать через DI ?
        private readonly int[,] _map;                              // Получать через DI ?

        private readonly GameState _gameState;
        private readonly EntitiesFactory _entitiesFactory;          // Получать через DI ?

        private readonly bool _isLoaded = false;

        public LevelCreator(DIContainer sceneContainer, ILevelConfig levelConfig)
        {
            _obstacleTileMap = sceneContainer.Resolve<Tilemap>(GameConstants.Obstacle);
            _obstacleTiles = LoadTiles(GameConstants.WallTilesFolderPath, GameConstants.NumberOfWallTiles);
            var gameStateService = sceneContainer.Resolve<IGameStateService>();
            _map = levelConfig.Map;
            _gameState = gameStateService.GameState;
            _entitiesFactory = _gameState.EntitiesFactory;
            _isLoaded = gameStateService.GameStateIsLoaded;

            ConstructLevel();
            Registrations(sceneContainer);

            gameStateService.SaveGameState();
        }

        private void Registrations(DIContainer sceneContainer)
        {
            var entities = _gameState.Map.Value.Entities
                .Where(entity => entity.Type <= EntityType.Pacman && entity.Type >= EntityType.Clyde).ToList();

            foreach (var entity in entities)
                sceneContainer.RegisterInstance(entity.Type.ToString(), entity);
        }

        private void ConstructLevel()
        {
            _obstacleTileMap.ClearAllTiles();

            for (int y = 0; y < _map.GetLength(0); y++)
            {
                for (int x = 0; x < _map.GetLength(1); x++)
                {
                    int numTile = _map[y, x];

                    if (numTile >= 0)
                    {
                        CreateObstacle(numTile, x, y);
                    }
                    else if(numTile <= (int)EntityType.SmallPellet)
                    {
                        if (numTile <= (int)EntityType.Pacman)
                        {
                            var position = CalculateCorrectSpawnPosition(numTile, x, y);
                            SetSpawnPosition((EntityType)numTile, position);
                        }

                        if (_isLoaded == false)
                            CreateEntity(x, y, (EntityType)numTile);
                    }
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
            var tile = numTile >= 0 ? _obstacleTiles[numTile] : null;
            _obstacleTileMap.SetTile(tilePos, tile);
        }

        private void CreateEntity(int x, int y, EntityType entityType)
        {
            var pos = new Vector2(x + GameConstants.Half, -y + GameConstants.Half);
            Entity entity = null;

            if (entityType <= EntityType.Pacman)
            {
                entity = _gameState.Map.Value.Entities.FirstOrDefault(entity => entity.Type == entityType);
                pos = GetSpawnPosition(entityType);
            }

            if (entity == null)
            {
                entity = _entitiesFactory.CreateEntity(pos, entityType);
                _gameState.Map.Value.Entities.Add(entity);
            }
        }

        private Vector2 CalculateCorrectSpawnPosition(int numTile, int x, int y)
        {
            var position = new Vector2(x + GameConstants.Half, -y + GameConstants.Half);

            if (_map[y + 1, x] == numTile)
                position.y -= GameConstants.Half;
            else if (_map[y - 1, x] == numTile)
                position.y += GameConstants.Half;
            else if (_map[y, x + 1] == numTile)
                position.x += GameConstants.Half;
            else if (_map[y, x - 1] == numTile)
                position.x -= GameConstants.Half;

            return position;
        }

        private void SetSpawnPosition(EntityType entityType, Vector2 spawnPosition)
        {
            switch (entityType)
            {
                case EntityType.Pacman:
                    _gameState.Map.CurrentValue.PacmanSpawnPos.Value = spawnPosition;
                    break;

                case EntityType.Blinky:
                    _gameState.Map.CurrentValue.BlinkySpawnPos.Value = spawnPosition;
                    break;

                case EntityType.Pinky:
                    _gameState.Map.CurrentValue.PinkySpawnPos.Value = spawnPosition;
                    break;

                case EntityType.Inky:
                    _gameState.Map.CurrentValue.InkySpawnPos.Value = spawnPosition;
                    break;

                case EntityType.Clyde:
                    _gameState.Map.CurrentValue.ClydeSpawnPos.Value = spawnPosition;
                    break;

                case EntityType.Fruit:
                    _gameState.Map.CurrentValue.FruitSpawnPos.Value = spawnPosition;
                    break;
            }
        }

        private Vector2 GetSpawnPosition(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Pacman:
                    return _gameState.Map.CurrentValue.PacmanSpawnPos.Value;

                case EntityType.Blinky:
                    return _gameState.Map.CurrentValue.BlinkySpawnPos.Value;

                case EntityType.Pinky:
                    return _gameState.Map.CurrentValue.PinkySpawnPos.Value;

                case EntityType.Inky:
                    return _gameState.Map.CurrentValue.InkySpawnPos.Value;

                case EntityType.Clyde:
                    return _gameState.Map.CurrentValue.ClydeSpawnPos.Value;

                case EntityType.Fruit:
                    return _gameState.Map.CurrentValue.FruitSpawnPos.Value;

                default:
                    throw new System.Exception($"Undefined type: {entityType}");
            }
        }
    }
}
