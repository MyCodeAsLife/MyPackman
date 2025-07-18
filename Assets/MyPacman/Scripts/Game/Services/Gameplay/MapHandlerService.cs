using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class MapHandlerService
    {
        private readonly ObservableList<Entity> _entities;
        private readonly GameState _gameState;

        private readonly TilemapHandler _tilemapHandler;

        private Vector2 _fruitSpawnPosition;

        // Добавить подписку на изменение данного массива, при изменении позиции призраков
        private readonly Dictionary<Vector3Int, Entity> _edibleEntityMap = new();       // Призраки не меняют свою позицию

        public event Action<EdibleEntityPoints> EntityEaten;

        public MapHandlerService(GameState gameState, ILevelConfig levelConfig, Tilemap obstaclesTileMap, PlayerMovemenService player)
        {
            _gameState = gameState;
            _entities = gameState.Map.CurrentValue.Entities;

            _tilemapHandler = new TilemapHandler(obstaclesTileMap, levelConfig);
            _gameState.Map.Value.FruitSpawnPos.Subscribe(position => _fruitSpawnPosition = position);
            player.PlayerTilePosition.Subscribe(PlayerTileChanged);
            gameState.Map.CurrentValue.NumberOfCollectedPellets.Subscribe(OnCollectedPellet);

            InitEdibleEntityMap();
        }

        public bool CheckTileForObstacle(Vector2 position) => _tilemapHandler.CheckTileForObstacle(position);
        public List<Vector2> GetDirectionsWithoutObstacles(Vector2 position) => _tilemapHandler.GetDirectionsWithoutObstacles(position);
        public List<Vector2> GetDirectionsWithoutWalls(Vector2 position) => _tilemapHandler.GetDirectionsWithoutWalls(position);
        public bool IsCenterTail(Vector2 position) => _tilemapHandler.IsCenterTail(position);
        public List<Vector2> GetTilePositions(int gateTile) => _tilemapHandler.GetTilePositions(gateTile);
        public bool CheckTile(Vector2 position, int numTile) => _tilemapHandler.CheckTile(position, numTile);

        private void PlayerTileChanged(Vector3Int position)
        {
            //var position = Convert.ToTilePosition(position);

            if (_edibleEntityMap.TryGetValue(position, out Entity entity))
            {
                var edibleEntity = entity as Edible;

                if (edibleEntity.Type <= EntityType.Blinky && edibleEntity.Type >= EntityType.Clyde)
                {
                    // Проверять возможность съесть призрака
                    // Отправлять запрос в CharactersStateHandler
                    // если проверка не пройденна return
                }
                else
                {
                    _entities.Remove(edibleEntity);
                    EntityEaten?.Invoke(edibleEntity.Points);
                }
            }
        }

        private void InitEdibleEntityMap()
        {
            foreach (var entity in _entities)
                if (entity.Type != EntityType.Pacman)
                    AddEntity(entity);


            _entities.ObserveAdd().Subscribe(e =>
            {
                if (e.Value.Type != EntityType.Pacman)
                    AddEntity(e.Value);
            });

            _entities.ObserveRemove().Subscribe(e =>
            {
                if (e.Value.Type != EntityType.Pacman)
                    RemoveEntity(e.Value);
            });
        }

        private void RemoveEntity(Entity entity)
        {
            var position = Convert.ToTilePosition(entity.Position.Value);
            _edibleEntityMap.Remove(position);
        }

        private void AddEntity(Entity entity)
        {
            var position = Convert.ToTilePosition(entity.Position.Value);
            _edibleEntityMap[position] = entity;
        }

        private void OnCollectedPellet(int numberOfCollectedPellets)
        {
            if (numberOfCollectedPellets == GameConstants.CollectedPelletsForFirstFruitSpawn ||
                numberOfCollectedPellets == GameConstants.CollectedPelletsForSecondFruitSpawn)
                SpawnFruit();
        }

        private void SpawnFruit()
        {
            int numFruitType = (int)EntityType.Chery - _gameState.NumberOfCollectedFruits.CurrentValue;

            if (numFruitType < (int)EntityType.Key)
                numFruitType = (int)EntityType.Key;

            var entity = _gameState.EntitiesFactory.CreateEntity(_fruitSpawnPosition, (EntityType)numFruitType);
            _gameState.Map.CurrentValue.Entities.Add(entity);
        }

        //public bool CheckTileForObstacle(Vector2 position)                          // Map
        //{
        //    var tilePosition = Convert.ToTilePosition(position);
        //    return CheckTileForObstacle(tilePosition);
        //}

        //public List<Vector2> GetDirectionsWithoutObstacles(Vector2 position)        // Map
        //{
        //    var tilePosition = Convert.ToTilePosition(position);
        //    List<Vector2> directions = new();

        //    if (CheckTileForObstacle(tilePosition + Vector3Int.left) == false)
        //        directions.Add(Vector2.left);

        //    if (CheckTileForObstacle(tilePosition + Vector3Int.right) == false)
        //        directions.Add(Vector2.right);

        //    if (CheckTileForObstacle(tilePosition + Vector3Int.up) == false)
        //        directions.Add(Vector2.up);

        //    if (CheckTileForObstacle(tilePosition + Vector3Int.down) == false)
        //        directions.Add(Vector2.down);

        //    return directions;
        //}

        //public List<Vector2> GetDirectionsWithoutWalls(Vector2 position)        // Map
        //{
        //    var tilePosition = Convert.ToTilePosition(position);
        //    List<Vector2> directions = GetDirectionsWithoutObstacles(position);

        //    if (CheckTileForType(tilePosition + Vector3Int.left, GameConstants.GateTile))
        //        directions.Add(Vector2.left);

        //    if (CheckTileForType(tilePosition + Vector3Int.right, GameConstants.GateTile))
        //        directions.Add(Vector2.right);

        //    if (CheckTileForType(tilePosition + Vector3Int.up, GameConstants.GateTile))
        //        directions.Add(Vector2.up);

        //    if (CheckTileForType(tilePosition + Vector3Int.down, GameConstants.GateTile))
        //        directions.Add(Vector2.down);

        //    return directions;
        //}

        //public bool IsCenterTail(Vector2 position)                          // Map
        //{
        //    var valueX = position.x - Mathf.Floor(position.x);
        //    var valueY = position.y - Mathf.Floor(position.y);

        //    if (Mathf.Approximately(valueX, GameConstants.Half) && Mathf.Approximately(valueY, GameConstants.Half))
        //        return true;

        //    return false;
        //}

        //public Vector2 GetTilePosition(int gateTile)                            // Map
        //{
        //    throw new NotImplementedException();
        //}

        //private bool CheckTileForObstacle(Vector3Int tilePos)                   // Map
        //{
        //    var tile = _obstaclesTileMap.GetTile(tilePos);
        //    return tile != null;
        //}

        //private bool CheckTileForType(Vector3Int tilePos, int tileType)         // Map
        //{
        //    var tile = _obstaclesTileMap.GetTile(tilePos);

        //    if (tile != null)
        //    {
        //        int numType = int.Parse(tile.name);
        //        return numType == tileType;
        //    }

        //    return false;
        //}

        //-------------------------------------------------------------------------------------------------

        //public List<Vector2> GetAvailableMovementPoints(Vector2 position)
        //{
        //    var tilePosition = Convert.ToTilePosition(position);
        //    List<Vector2> points = new List<Vector2>();

        //    for (int x = -1; x < 2; x += 2)
        //    {
        //        var checkPosition = new Vector3Int(tilePosition.y + x, tilePosition.y, tilePosition.z);
        //        TryAddAvailablePoint(checkPosition, points);
        //    }

        //    for (int y = -1; y < 2; y += 2)
        //    {
        //        var checkPosition = new Vector3Int(tilePosition.x, tilePosition.y - y, tilePosition.z);
        //        TryAddAvailablePoint(checkPosition, points);
        //    }

        //    return points;
        //}

        //private void TryAddAvailablePoint(Vector3Int checkPosition, List<Vector2> points)
        //{
        //    var tile = _obstaclesTileMap.GetTile(checkPosition);

        //    if (tile != null)
        //    {
        //        var availablePosition = new Vector2(
        //            checkPosition.x + GameConstants.Half,
        //            checkPosition.y - GameConstants.Half);
        //        points.Add(availablePosition);
        //    }
        //}

        //public bool IsIntersactionTile(int x, int y)       // Проверка на перекресток
        //{
        //    int numberOfPaths = 0;
        //    int vertical = 0;
        //    int horizontal = 0;
        //    int maxLengthY = _currentLevel.Map.GetLength(0);
        //    int maxLengthX = _currentLevel.Map.GetLength(1);

        //    int leftX = x - 1;
        //    int rigthX = x + 1;
        //    int downY = y - 1;
        //    int upY = y + 1;

        //    if (leftX >= 0 && (_currentLevel.Map[y, leftX] == GameConstants.SmallPellet ||
        //                     _currentLevel.Map[y, leftX] == GameConstants.EmptyTile))
        //    {
        //        numberOfPaths++;
        //        vertical++;
        //    }

        //    if (rigthX < maxLengthX && (_currentLevel.Map[y, rigthX] == GameConstants.SmallPellet ||
        //                               _currentLevel.Map[y, rigthX] == GameConstants.EmptyTile))
        //    {
        //        numberOfPaths++;
        //        vertical--;
        //    }

        //    if (downY >= 0 && (_currentLevel.Map[downY, x] == GameConstants.SmallPellet ||
        //                       _currentLevel.Map[downY, x] == GameConstants.EmptyTile))
        //    {
        //        numberOfPaths++;
        //        horizontal++;
        //    }

        //    if (upY < maxLengthY && (_currentLevel.Map[upY, x] == GameConstants.SmallPellet ||
        //                                _currentLevel.Map[upY, x] == GameConstants.EmptyTile))
        //    {
        //        numberOfPaths++;
        //        horizontal--;
        //    }

        //    return numberOfPaths > 2 || horizontal != 0 || vertical != 0 ? true : false;                                    //Magic
        //}
    }
}