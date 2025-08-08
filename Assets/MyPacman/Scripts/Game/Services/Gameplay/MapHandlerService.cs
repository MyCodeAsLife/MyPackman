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

        public event Action<EdibleEntityPoints, Vector2> EntityEaten;

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
                    EntityEaten?.Invoke(edibleEntity.Points, edibleEntity.Position.Value);
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
    }
}