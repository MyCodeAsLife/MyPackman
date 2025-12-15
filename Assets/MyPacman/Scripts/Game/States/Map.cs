using ObservableCollections;
using R3;
using System;
using System.Linq;
using UnityEngine;

namespace MyPacman
{
    public class Map
    {
        public readonly ReactiveProperty<int> LevelNumber;
        public readonly ReactiveProperty<int> ScoreForRound;                    // Обнулять при сменен уровня
        public readonly ReactiveProperty<int> NumberOfPellets;
        public readonly ReactiveProperty<int> NumberOfCollectedPellets;         // Обнулять при сменен уровня ????
        public readonly ReactiveProperty<float> TimeLeftUntilEndOfFearMode;     // Обнулять при при сменен уровня и окончании действия поведения
        public readonly ReactiveProperty<float> GlobalBehaviorModeChangeTimer;  // Обнулять при сменен уровня и смерти игрока

        public readonly ReactiveProperty<Vector2> PacmanSpawnPos;
        public readonly ReactiveProperty<Vector2> BlinkySpawnPos;
        public readonly ReactiveProperty<Vector2> PinkySpawnPos;
        public readonly ReactiveProperty<Vector2> InkySpawnPos;
        public readonly ReactiveProperty<Vector2> ClydeSpawnPos;
        public readonly ReactiveProperty<Vector2> FruitSpawnPos;

        public readonly ReactiveProperty<GhostBehaviorModeType> GlobalStateOfBehavior;

        private readonly EntitiesFactory _entitiesFactory;

        public Map(MapData mapData, Func<int> createEntityId)
        {
            OriginData = mapData;
            MapTag = mapData.MapTag;
            _entitiesFactory = new EntitiesFactory(createEntityId);
            OriginData.NumberOfPellets = 0;

            //LevelNumber = InitLevelNumber();                                    // Свернуть в шаблонную функцию с входящим параметром?
            //ScoreForRound = InitScoreForRound();
            //NumberOfPellets = InitNumberOfPellets();
            //GlobalStateOfBehavior = InitGlobalStateOfBehavior();
            //NumberOfCollectedPellets = InitNumberOfCollectedPellets();
            //TimeLeftUntilEndOfFearMode = InitTimeLeftUntilEndOfFearMode();
            //GlobalBehaviorModeChangeTimer = InitGlobalBehaviorModeChangeTimer();
            //PacmanSpawnPos = InitPacmanSpawnPos();
            //BlinkySpawnPos = InitBlinkySpawnPos();
            //PinkySpawnPos = InitPinkySpawnPos();
            //InkySpawnPos = InitInkySpawnPos();
            //ClydeSpawnPos = InitClydeSpawnPos();
            //FruitSpawnPos = InitFruitSpawnPos();

            LevelNumber = new ReactiveProperty<int>(OriginData.LevelNumber);
            ScoreForRound = new ReactiveProperty<int>(OriginData.ScoreForRound);
            NumberOfPellets = new ReactiveProperty<int>(OriginData.NumberOfPellets);
            NumberOfCollectedPellets = new ReactiveProperty<int>(OriginData.NumberOfCollectedPellets);
            TimeLeftUntilEndOfFearMode = new ReactiveProperty<float>(OriginData.TimeLeftUntilEndOfFearMode);
            GlobalBehaviorModeChangeTimer = new ReactiveProperty<float>(OriginData.GlobalBehaviorModeChangeTimer);
            GlobalStateOfBehavior = new ReactiveProperty<GhostBehaviorModeType>(OriginData.GlobalStateOfBehavior);
            PacmanSpawnPos = new ReactiveProperty<Vector2>(new Vector2(OriginData.PacmanSpawnPosX, OriginData.PacmanSpawnPosY));
            BlinkySpawnPos = new ReactiveProperty<Vector2>(new Vector2(OriginData.BlinkySpawnPosX, OriginData.BlinkySpawnPosY));
            PinkySpawnPos = new ReactiveProperty<Vector2>(new Vector2(OriginData.PinkySpawnPosX, OriginData.PinkySpawnPosY));
            InkySpawnPos = new ReactiveProperty<Vector2>(new Vector2(OriginData.InkySpawnPosX, OriginData.InkySpawnPosY));
            ClydeSpawnPos = new ReactiveProperty<Vector2>(new Vector2(OriginData.ClydeSpawnPosX, OriginData.ClydeSpawnPosY));
            FruitSpawnPos = new ReactiveProperty<Vector2>(new Vector2(OriginData.FruitSpawnPosX, OriginData.FruitSpawnPosY));

            SubscribeProperties();
            InitEntityCounting();
            InitEntities(mapData);
        }

        public MapData OriginData { get; }
        public string MapTag { get; }
        public ObservableList<Entity> Entities { get; } = new();

        public Entity CreateEntity(Vector2 position, EntityType entityType)
        {
            var entity = _entitiesFactory.CreateNewEntity(position, entityType);
            Entities.Add(entity);
            return entity;
        }

        public void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);
        }

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
            switch (entityType)
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

        private void SubscribeProperties()
        {
            LevelNumber.Subscribe(value => OriginData.LevelNumber = value);
            ScoreForRound.Subscribe(value => OriginData.ScoreForRound = value);
            NumberOfPellets.Subscribe(value => OriginData.NumberOfPellets = value);
            GlobalStateOfBehavior.Subscribe(value => OriginData.GlobalStateOfBehavior = value);
            NumberOfCollectedPellets.Subscribe(value => OriginData.NumberOfCollectedPellets = value);
            TimeLeftUntilEndOfFearMode.Subscribe(value => OriginData.TimeLeftUntilEndOfFearMode = value);
            GlobalBehaviorModeChangeTimer.Subscribe(value => OriginData.GlobalBehaviorModeChangeTimer = value);
            PacmanSpawnPos.Subscribe(value =>
                {
                    OriginData.PacmanSpawnPosX = value.x;
                    OriginData.PacmanSpawnPosY = value.y;
                });
            BlinkySpawnPos.Subscribe(value =>
                {
                    OriginData.BlinkySpawnPosX = value.x;
                    OriginData.BlinkySpawnPosY = value.y;
                });
            PinkySpawnPos.Subscribe(value =>
                {
                    OriginData.PinkySpawnPosX = value.x;
                    OriginData.PinkySpawnPosY = value.y;
                });
            InkySpawnPos.Subscribe(value =>
                {
                    OriginData.InkySpawnPosX = value.x;
                    OriginData.InkySpawnPosY = value.y;
                });
            ClydeSpawnPos.Subscribe(value =>
                {
                    OriginData.ClydeSpawnPosX = value.x;
                    OriginData.ClydeSpawnPosY = value.y;
                });
            FruitSpawnPos.Subscribe(value =>
                {
                    OriginData.FruitSpawnPosX = value.x;
                    OriginData.FruitSpawnPosY = value.y;
                });
        }

        private void InitEntityCounting()
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
            mapData.Entities.ForEach(entityData => Entities.Add(_entitiesFactory.CreateEntityBasedOnData(entityData)));

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

        //private ReactiveProperty<int> InitLevelNumber()
        //{
        //    var LevelNumber = new ReactiveProperty<int>(OriginData.LevelNumber);
        //    LevelNumber.Subscribe(value => OriginData.LevelNumber = value);
        //    return LevelNumber;
        //}

        //private ReactiveProperty<int> InitScoreForRound()
        //{
        //    var ScoreForRound = new ReactiveProperty<int>(OriginData.ScoreForRound);
        //    ScoreForRound.Subscribe(value => OriginData.ScoreForRound = value);
        //    return ScoreForRound;
        //}

        //private ReactiveProperty<int> InitNumberOfPellets()
        //{
        //    OriginData.NumberOfPellets = 0;
        //    var NumberOfPellets = new ReactiveProperty<int>(OriginData.NumberOfPellets);
        //    NumberOfPellets.Subscribe(value => OriginData.NumberOfPellets = value);
        //    return NumberOfPellets;
        //}

        //private ReactiveProperty<int> InitNumberOfCollectedPellets()
        //{
        //    var NumberOfCollectedPellets = new ReactiveProperty<int>(OriginData.NumberOfCollectedPellets);
        //    NumberOfCollectedPellets.Subscribe(value => OriginData.NumberOfCollectedPellets = value);
        //    return NumberOfCollectedPellets;
        //}

        //private ReactiveProperty<float> InitTimeLeftUntilEndOfFearMode()
        //{
        //    var reactiveProperty = new ReactiveProperty<float>(OriginData.TimeLeftUntilEndOfFearMode);
        //    reactiveProperty.Subscribe(value => OriginData.TimeLeftUntilEndOfFearMode = value);
        //    return reactiveProperty;
        //}

        //private ReactiveProperty<GhostBehaviorModeType> InitGlobalStateOfBehavior()
        //{
        //    var reactiveProperty = new ReactiveProperty<GhostBehaviorModeType>(OriginData.GlobalStateOfBehavior);
        //    reactiveProperty.Subscribe(value => OriginData.GlobalStateOfBehavior = value);
        //    return reactiveProperty;
        //}

        //private ReactiveProperty<float> InitGlobalBehaviorModeChangeTimer()
        //{
        //    var reactiveProperty = new ReactiveProperty<float>(OriginData.GlobalBehaviorModeChangeTimer);
        //    reactiveProperty.Subscribe(value => OriginData.GlobalBehaviorModeChangeTimer = value);
        //    return reactiveProperty;
        //}

        //private ReactiveProperty<Vector2> InitPacmanSpawnPos()
        //{
        //    var pacmanSpawnPos = new ReactiveProperty<Vector2>(new Vector2(
        //        OriginData.PacmanSpawnPosX,
        //        OriginData.PacmanSpawnPosY));
        //    pacmanSpawnPos.Subscribe(newPos =>
        //    {
        //        OriginData.PacmanSpawnPosX = newPos.x;
        //        OriginData.PacmanSpawnPosY = newPos.y;
        //    });

        //    return pacmanSpawnPos;
        //}

        //private ReactiveProperty<Vector2> InitBlinkySpawnPos()
        //{
        //    var blinkySpawnPos = new ReactiveProperty<Vector2>(new Vector2(
        //        OriginData.BlinkySpawnPosX,
        //        OriginData.BlinkySpawnPosY));
        //    blinkySpawnPos.Subscribe(newPos =>
        //    {
        //        OriginData.BlinkySpawnPosX = newPos.x;
        //        OriginData.BlinkySpawnPosY = newPos.y;
        //    });

        //    return blinkySpawnPos;
        //}

        //private ReactiveProperty<Vector2> InitPinkySpawnPos()
        //{
        //    var pinkySpawnPos = new ReactiveProperty<Vector2>(new Vector2(
        //        OriginData.PinkySpawnPosX,
        //        OriginData.PinkySpawnPosY));
        //    pinkySpawnPos.Subscribe(newPos =>
        //    {
        //        OriginData.PinkySpawnPosX = newPos.x;
        //        OriginData.PinkySpawnPosY = newPos.y;
        //    });

        //    return pinkySpawnPos;
        //}

        //private ReactiveProperty<Vector2> InitInkySpawnPos()
        //{
        //    var inkySpawnPos = new ReactiveProperty<Vector2>(new Vector2(
        //        OriginData.InkySpawnPosX,
        //        OriginData.InkySpawnPosY));
        //    inkySpawnPos.Subscribe(newPos =>
        //    {
        //        OriginData.InkySpawnPosX = newPos.x;
        //        OriginData.InkySpawnPosY = newPos.y;
        //    });

        //    return inkySpawnPos;
        //}

        //private ReactiveProperty<Vector2> InitClydeSpawnPos()
        //{
        //    var clydeSpawnPos = new ReactiveProperty<Vector2>(new Vector2(
        //        OriginData.ClydeSpawnPosX,
        //        OriginData.ClydeSpawnPosY));
        //    clydeSpawnPos.Subscribe(newPos =>
        //    {
        //        OriginData.ClydeSpawnPosX = newPos.x;
        //        OriginData.ClydeSpawnPosY = newPos.y;
        //    });

        //    return clydeSpawnPos;
        //}

        //private ReactiveProperty<Vector2> InitFruitSpawnPos()
        //{
        //    var fruitSpawnPos = new ReactiveProperty<Vector2>(new Vector2(
        //        OriginData.FruitSpawnPosX,
        //        OriginData.FruitSpawnPosY));

        //    fruitSpawnPos.Subscribe(newPos =>
        //    {
        //        OriginData.FruitSpawnPosX = newPos.x;
        //        OriginData.FruitSpawnPosY = newPos.y;
        //    });

        //    return fruitSpawnPos;
        //}

        //private ReactiveProperty<T> InitSimpleReactiveProperty<T>(Func<T> propertyGetter, Action<T> propertySetter)
        //{
        //    var newReactiveProperty = new ReactiveProperty<T>(propertyGetter());
        //    newReactiveProperty.Subscribe(value => propertySetter(value));
        //    return newReactiveProperty;
        //}

        //private ReactiveProperty<Vector2> InitVector2ReactiveProperty(
        //    Func<Vector2> propertyGetter,
        //    Action<Vector2> propertySetter)
        //{
        //    var newReactiveProperty = new ReactiveProperty<Vector2>(propertyGetter());
        //    newReactiveProperty.Subscribe(value => propertySetter(value));
        //    return newReactiveProperty;
        //}
    }
}