using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class HandlerOfPickedEntities
    {
        private readonly GameState _gameState;
        private readonly TilemapHandler _tilemapHandler;
        private readonly TimeService _timeService;
        private readonly IObservableCollection<Entity> _entities;
        private readonly ReadOnlyReactiveProperty<Vector2> _fruitSpawnPosition;
        private readonly Dictionary<Vector3Int, Entity> _edibleEntityMap = new();

        private EntityType _currentFruitType;

        public event Action<int, Vector2> EntityEaten;

        public HandlerOfPickedEntities(
            GameState gameState,
            ILevelConfig levelConfig,
            Tilemap obstaclesTileMap,
            PacmanMovementService player,
            TimeService timeService)
        {
            _gameState = gameState;
            _entities = gameState.Map.CurrentValue.Entities;
            _fruitSpawnPosition = _gameState.Map.Value.FruitSpawnPos;
            _tilemapHandler = new TilemapHandler(obstaclesTileMap, levelConfig);
            _timeService = timeService;
            player.PlayerTilePosition.Subscribe(PlayerTileChanged);
            gameState.Map.CurrentValue.NumberOfCollectedPellets.Subscribe(OnCollectedPellet);

            InitEdibleEntityMap();
        }

        public IReadOnlyList<Vector2> SpeedModifierPositions => _tilemapHandler.SpeedModifierPositions;
        public bool CheckTileForObstacle(Vector2 position) => _tilemapHandler.CheckTileForObstacle(position);   // Упразднить, перевести на CheckTile?
        public bool CheckTile(Vector2 position, int numTile) => _tilemapHandler.CheckTile(position, numTile);
        public List<Vector2> GetDirectionsWithoutObstacles(Vector2 position) => _tilemapHandler.GetDirectionsWithoutObstacles(position);
        public List<Vector2> GetDirectionsWithoutWalls(Vector2 position) => _tilemapHandler.GetDirectionsWithoutWalls(position);
        public bool IsCenterOfTile(Vector2 position) => _tilemapHandler.IsCenterOfTile(position);

        private void PlayerTileChanged(Vector3Int position)
        {
            if (_edibleEntityMap.TryGetValue(position, out Entity entity))
            {
                var edibleEntity = entity as Edible;
                _gameState.Map.CurrentValue.RemoveEntity(edibleEntity);
                EntityEaten?.Invoke((int)edibleEntity.Points, edibleEntity.Position.Value);
            }
        }

        private void InitEdibleEntityMap()
        {
            foreach (var entity in _entities)
                if (entity.Type > EntityType.Pacman || entity.Type < EntityType.Clyde)
                    AddEdibleEntity(entity);

            _entities.ObserveAdd().Subscribe(e =>
            {
                if (e.Value.Type != EntityType.Pacman)
                    AddEdibleEntity(e.Value);
            });

            _entities.ObserveRemove().Subscribe(e =>
            {
                if (e.Value.Type != EntityType.Pacman)
                    RemoveEdibleEntity(e.Value);
            });
        }

        private void RemoveEdibleEntity(Entity entity)
        {
            var position = Convert.ToTilePosition(entity.Position.Value);
            _edibleEntityMap.Remove(position);
        }

        private void AddEdibleEntity(Entity entity)
        {
            var position = Convert.ToTilePosition(entity.Position.Value);
            _edibleEntityMap[position] = entity;
        }

        private void OnCollectedPellet(int numberOfCollectedPellets)
        {
            if (numberOfCollectedPellets == GameConstants.CollectedPelletsForFirstFruitSpawn ||
                numberOfCollectedPellets == GameConstants.CollectedPelletsForSecondFruitSpawn)
            {
                SpawnFruit();
                InitFruit();
            }
        }

        private void SpawnFruit()
        {
            _currentFruitType = (EntityType)((int)EntityType.Cherry - _gameState.PickedFruits.Count);  // Если игрок не будет подбирать бонусы, то они не будут улучшатся

            if (_currentFruitType < EntityType.Key)
                _currentFruitType = EntityType.Key;

            _gameState.Map.CurrentValue.CreateEntity(_fruitSpawnPosition.CurrentValue, _currentFruitType);
        }

        private void InitFruit()
        {
            Fruit fruit = GetFruit(_currentFruitType);
            fruit.Init(_timeService);
            fruit.TimeOfLifeIsOver += OnFruitTimeOfLifeOver;
        }

        private Fruit GetFruit(EntityType fruitType)
        {
            foreach (var fruit in _edibleEntityMap.Values)
                if (fruit.Type == fruitType)
                    return (Fruit)fruit;

            throw new Exception($"Invalid fruit type: {fruitType}");        // Magic
        }

        private void OnFruitTimeOfLifeOver(Fruit fruit)
        {
            fruit.TimeOfLifeIsOver -= OnFruitTimeOfLifeOver;
            _gameState.Map.CurrentValue.RemoveEntity(fruit);    // Зачисление на панель съеденного фрукта подписанно на его удаление? Переделать на здешний ивент по подбору
        }
    }
}