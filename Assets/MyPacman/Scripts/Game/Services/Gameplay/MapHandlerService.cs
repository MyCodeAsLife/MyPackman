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
        private readonly GameState _gameState;
        private readonly TilemapHandler _tilemapHandler;
        private readonly ObservableList<Entity> _entities;
        private readonly ReadOnlyReactiveProperty<Vector2> _fruitSpawnPosition;
        private readonly Dictionary<Vector3Int, Entity> _edibleEntityMap = new();

        public event Action<EdibleEntityPoints, Vector2> EntityEaten;

        public MapHandlerService(GameState gameState, ILevelConfig levelConfig, Tilemap obstaclesTileMap, PlayerMovementService player)
        {
            _gameState = gameState;
            _entities = gameState.Map.CurrentValue.Entities;
            _fruitSpawnPosition = _gameState.Map.Value.FruitSpawnPos;
            _tilemapHandler = new TilemapHandler(obstaclesTileMap, levelConfig);
            player.PlayerTilePosition.Subscribe(PlayerTileChanged);
            gameState.Map.CurrentValue.NumberOfCollectedPellets.Subscribe(OnCollectedPellet);

            InitEdibleEntityMap();
        }

        public IReadOnlyList<Vector2> GatePositions => _tilemapHandler.GatePositions;
        public IReadOnlyList<Vector2> SpeedModifierPositions => _tilemapHandler.SpeedModifierPositions;
        public bool CheckTileForObstacle(Vector2 position) => _tilemapHandler.CheckTileForObstacle(position);
        public List<Vector2> GetDirectionsWithoutObstacles(Vector2 position) => _tilemapHandler.GetDirectionsWithoutObstacles(position);
        public List<Vector2> GetDirectionsWithoutWalls(Vector2 position) => _tilemapHandler.GetDirectionsWithoutWalls(position);
        public bool IsCenterTail(Vector2 position) => _tilemapHandler.IsCenterTail(position);
        public bool CheckTile(Vector2 position, int numTile) => _tilemapHandler.CheckTile(position, numTile);

        private void PlayerTileChanged(Vector3Int position)
        {
            if (_edibleEntityMap.TryGetValue(position, out Entity entity))
            {
                var edibleEntity = entity as Edible;            // Нужен ли класс Edible ???
                _entities.Remove(edibleEntity);
                EntityEaten?.Invoke(edibleEntity.Points, edibleEntity.Position.Value);
            }
        }

        private void InitEdibleEntityMap()
        {
            foreach (var entity in _entities)
                if (entity.Type > EntityType.Pacman || entity.Type < EntityType.Clyde)
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
            int numFruitType = (int)EntityType.Cherry - _gameState.PickedFruits.Count;  // Если игрок не будет подбирать бонусы, то они не будут улучшатся

            if (numFruitType < (int)EntityType.Key)
                numFruitType = (int)EntityType.Key;

            var entity = _gameState.EntitiesFactory.CreateEntity(_fruitSpawnPosition.CurrentValue, (EntityType)numFruitType);
            _gameState.Map.CurrentValue.Entities.Add(entity);
        }
    }
}