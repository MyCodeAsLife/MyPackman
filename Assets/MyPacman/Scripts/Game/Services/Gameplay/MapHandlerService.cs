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
        private readonly Tilemap _obstaclesTileMap;
        private readonly GameState _gameState;

        private Vector2 _fruitSpawnPosition;
        private readonly Dictionary<Vector3Int, Entity> _edibleEntityMap = new();

        public event Action<EdibleEntityPoints> EntityEaten;
        // 3. проверки позиций на препятствия?

        public MapHandlerService(GameState gameState, Tilemap obstaclesTileMap, PlayerMovemenService player)
        {
            _gameState = gameState;
            _entities = gameState.Map.CurrentValue.Entities;
            _obstaclesTileMap = obstaclesTileMap;

            //_fruitSpawnPosition = _gameState.Map.Value.FruitSpawnPos.Value;           // Это лишнее, при подписке должно подтянутся значение
            _gameState.Map.Value.FruitSpawnPos.Subscribe(position => _fruitSpawnPosition = position);
            player.PlayerTilePosition.Subscribe(PlayerTileChanged);

            gameState.Map.CurrentValue.NumberOfCollectedPellets.Subscribe(OnCollectedPellet);

            InitEdibleEntityMap();
        }

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

        public bool CheckForObstacles(Vector2 position)
        {
            var tilePosition = Convert.ToTilePosition(position);
            var tile = _obstaclesTileMap.GetTile(tilePosition);

            if (tile != null && int.Parse(tile.name) > 0)
                return true;

            return false;
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

        //private void ChangeTile(Vector3Int tilePosition, int objectNumber)
        //{
        //    //var handlePosition = ConvertToCellPosition(position);

        //    _currentLevel.Map[tilePosition.y, tilePosition.x] = objectNumber;                       // Изменяет Модель

        //    if (objectNumber > 0)
        //    {
        //        _obstaclesTileMap.SetTile(new Vector3Int(tilePosition.x, -tilePosition.y), null);        // Изменяет Presenter
        //    }
        //    else
        //    {
        //        _ediblesTilemap.SetTile(new Vector3Int(tilePosition.x, -tilePosition.y), null);
        //    }
        //}
    }
}
