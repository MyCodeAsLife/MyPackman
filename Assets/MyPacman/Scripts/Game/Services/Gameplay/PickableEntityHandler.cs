using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class PickableEntityHandler
    {
        private readonly GameState _gameState;
        private readonly TilemapHandler _tilemapHandler;
        private readonly TimeService _timeService;
        private readonly IObservableCollection<Entity> _entities;
        private readonly ReadOnlyReactiveProperty<Vector2> _fruitSpawnPosition;
        private readonly Dictionary<Vector3Int, Entity> _edibleEntityMap = new();

        public event Action<int, Vector2> EntityEaten;

        public PickableEntityHandler(
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
            FruitStartInit();
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

                if (edibleEntity.Type <= EntityType.Cherry)
                    _gameState.PickedFruits.Add(edibleEntity.Type);
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

        private void FruitStartInit()
        {
            foreach (var entity in _edibleEntityMap)
                if (entity.Value.Type <= EntityType.Cherry)
                    InitFruit(entity.Value as Fruit);
        }

        private void InitFruit(Fruit fruit)
        {
            fruit.TimeOfLifeIsOver += OnFruitTimeOfLifeOver;
            fruit.Init(_timeService);
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

        private Fruit SpawnFruit()
        {
            EntityType fruitType = (EntityType)((int)EntityType.Cherry - _gameState.PickedFruits.Count);

            if (fruitType < EntityType.Key)
                fruitType = EntityType.Key;

            Entity entity = _gameState.Map.CurrentValue.CreateEntity(_fruitSpawnPosition.CurrentValue, fruitType);
            return entity as Fruit;
        }

        private void OnCollectedPellet(int numberOfCollectedPellets)
        {
            if (numberOfCollectedPellets == GameConstants.CollectedPelletsForFirstFruitSpawn ||
                numberOfCollectedPellets == GameConstants.CollectedPelletsForSecondFruitSpawn)
            {
                Fruit fruit = SpawnFruit();
                InitFruit(fruit);
            }
        }

        private void OnFruitTimeOfLifeOver(Fruit fruit)
        {
            fruit.TimeOfLifeIsOver -= OnFruitTimeOfLifeOver;
            _gameState.Map.CurrentValue.RemoveEntity(fruit);
        }
    }
}