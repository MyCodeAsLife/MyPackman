using ObservableCollections;
using R3;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    public class Map
    {
        public readonly ReactiveProperty<int> LevelNumber;
        public readonly ReactiveProperty<int> NumberOfPellets;
        public readonly ReactiveProperty<int> NumberOfCollectedPellets;

        public readonly ReactiveProperty<Vector2> PacmanSpawnPos;
        public readonly ReactiveProperty<Vector2> BlinkySpawnPos;
        public readonly ReactiveProperty<Vector2> PinkySpawnPos;
        public readonly ReactiveProperty<Vector2> InkySpawnPos;
        public readonly ReactiveProperty<Vector2> ClydeSpawnPos;
        public readonly ReactiveProperty<Vector2> FruitSpawnPos;

        private readonly EntitiesFactory _entitiesFactory;

        public Map(MapData mapData, EntitiesFactory entitiesFactory)
        {
            OriginData = mapData;
            MapTag = mapData.MapTag;
            _entitiesFactory = entitiesFactory;

            InitCounters();
            InitEntities(mapData);

            PacmanSpawnPos = InitPacmanSpawnPos();
            BlinkySpawnPos = InitBlinkySpawnPos();
            PinkySpawnPos = InitPinkySpawnPos();
            InkySpawnPos = InitInkySpawnPos();
            ClydeSpawnPos = InitClydeSpawnPos();
            FruitSpawnPos = InitFruitSpawnPos();

            LevelNumber = new ReactiveProperty<int>(mapData.LevelNumber);
            NumberOfPellets = new ReactiveProperty<int>(mapData.NumberOfPellets);
            NumberOfCollectedPellets = new ReactiveProperty<int>(mapData.NumberOfCollectedPellets);
        }

        public MapData OriginData { get; }
        public string MapTag { get; }
        public ObservableList<Entity> Entities { get; } = new();

        public void SetSpawnPosition(SpawnPointType entityType, Vector2 spawnPosition)
        {
            switch (entityType)
            {
                case SpawnPointType.Pacman:
                    PacmanSpawnPos.Value = spawnPosition;
                    break;

                case SpawnPointType.Blinky:
                    BlinkySpawnPos.Value = spawnPosition;
                    break;

                case SpawnPointType.Pinky:
                    PinkySpawnPos.Value = spawnPosition;
                    break;

                case SpawnPointType.Inky:
                    InkySpawnPos.Value = spawnPosition;
                    break;

                case SpawnPointType.Clyde:
                    ClydeSpawnPos.Value = spawnPosition;
                    break;

                case SpawnPointType.Fruit:
                    FruitSpawnPos.Value = spawnPosition;
                    break;

                default:
                    throw new System.Exception($"Undefined type: {entityType}");            // Magic
            }
        }

        public Vector2 GetSpawnPosition(SpawnPointType entityType)
        {
            switch ((SpawnPointType)entityType)
            {
                case SpawnPointType.Pacman:
                    return PacmanSpawnPos.Value;

                case SpawnPointType.Blinky:
                    return BlinkySpawnPos.Value;

                case SpawnPointType.Pinky:
                    return PinkySpawnPos.Value;

                case SpawnPointType.Inky:
                    return InkySpawnPos.Value;

                case SpawnPointType.Clyde:
                    return ClydeSpawnPos.Value;

                case SpawnPointType.Fruit:
                    return FruitSpawnPos.Value;

                default:
                    throw new System.Exception($"Undefined type: {entityType}");            // Magic
            }
        }

        private void InitCounters()
        {
            Entities.ObserveAdd().Subscribe(collectionAddEvent =>
            {
                var addedEntity = collectionAddEvent.Value;

                if (addedEntity.Type <= EntityType.SmallPellet && addedEntity.Type >= EntityType.LargePellet)
                    NumberOfPellets.Value++;
            });

            Entities.ObserveRemove().Subscribe(collectionRemovedEvent =>
            {
                var removedEntity = collectionRemovedEvent.Value;

                if (removedEntity.Type <= EntityType.SmallPellet && removedEntity.Type >= EntityType.LargePellet)
                {
                    NumberOfPellets.Value--;
                    NumberOfCollectedPellets.Value++;
                }
            });
        }

        private void InitEntities(MapData mapData)
        {
            mapData.Entities.ForEach(entityData => Entities.Add(_entitiesFactory.CreateEntity(entityData)));

            // При добавлении элемента в Entities текущего класса, добавится элемент в Entities класса MapData
            Entities.ObserveAdd().Subscribe(collectionAddEvent =>
            {
                var addedEntity = collectionAddEvent.Value;
                mapData.Entities.Add(addedEntity.Origin);
            });

            // При удалении элемента из Entities текущего класса, также удалится элемент из Entities класса MapData
            Entities.ObserveRemove().Subscribe(collectionRemovedEvent =>
            {
                var removedEntity = collectionRemovedEvent.Value;
                var removedEntityData = mapData.Entities.FirstOrDefault(entityData =>
                                                    entityData.UniqId == removedEntity.UniqueId);
                mapData.Entities.Remove(removedEntityData);
            });
        }

        private ReactiveProperty<Vector2> InitPacmanSpawnPos()
        {
            var pacmanSpawnPos = new ReactiveProperty<Vector2>(new Vector2(
                OriginData.PacmanSpawnPosX,
                OriginData.PacmanSpawnPosY));
            pacmanSpawnPos.Subscribe(newPos =>
            {
                OriginData.PacmanSpawnPosX = newPos.x;
                OriginData.PacmanSpawnPosY = newPos.y;
            });

            return pacmanSpawnPos;
        }

        private ReactiveProperty<Vector2> InitBlinkySpawnPos()
        {
            var blinkySpawnPos = new ReactiveProperty<Vector2>(new Vector2(
                OriginData.BlinkySpawnPosX,
                OriginData.BlinkySpawnPosY));
            blinkySpawnPos.Subscribe(newPos =>
            {
                OriginData.BlinkySpawnPosX = newPos.x;
                OriginData.BlinkySpawnPosY = newPos.y;
            });

            return blinkySpawnPos;
        }

        private ReactiveProperty<Vector2> InitPinkySpawnPos()
        {
            var pinkySpawnPos = new ReactiveProperty<Vector2>(new Vector2(
                OriginData.PinkySpawnPosX,
                OriginData.PinkySpawnPosY));
            pinkySpawnPos.Subscribe(newPos =>
            {
                OriginData.PinkySpawnPosX = newPos.x;
                OriginData.PinkySpawnPosY = newPos.y;
            });

            return pinkySpawnPos;
        }

        private ReactiveProperty<Vector2> InitInkySpawnPos()
        {
            var inkySpawnPos = new ReactiveProperty<Vector2>(new Vector2(
                OriginData.InkySpawnPosX,
                OriginData.InkySpawnPosY));
            inkySpawnPos.Subscribe(newPos =>
            {
                OriginData.InkySpawnPosX = newPos.x;
                OriginData.InkySpawnPosY = newPos.y;
            });

            return inkySpawnPos;
        }

        private ReactiveProperty<Vector2> InitClydeSpawnPos()
        {
            var clydeSpawnPos = new ReactiveProperty<Vector2>(new Vector2(
                OriginData.ClydeSpawnPosX,
                OriginData.ClydeSpawnPosY));
            clydeSpawnPos.Subscribe(newPos =>
            {
                OriginData.ClydeSpawnPosX = newPos.x;
                OriginData.ClydeSpawnPosY = newPos.y;
            });

            return clydeSpawnPos;
        }

        private ReactiveProperty<Vector2> InitFruitSpawnPos()
        {
            var fruitSpawnPos = new ReactiveProperty<Vector2>(new Vector2(
                OriginData.FruitSpawnPosX,
                OriginData.FruitSpawnPosY));

            fruitSpawnPos.Subscribe(newPos =>
            {
                OriginData.FruitSpawnPosX = newPos.x;
                OriginData.FruitSpawnPosY = newPos.y;
            });

            return fruitSpawnPos;
        }
    }
}