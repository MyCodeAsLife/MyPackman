using ObservableCollections;
using R3;
using System.Linq;

namespace MyPacman
{
    public class Map
    {
        public readonly ReactiveProperty<int> LevelNumber;
        public readonly ReactiveProperty<int> NumberOfPellets;
        public readonly ReactiveProperty<int> NumberOfCollectedPellets;
        //public readonly ReactiveProperty<int> NumberOfCollectedFruits;

        private readonly EntitiesFactory _entitiesFactory;

        public Map(MapData mapData, EntitiesFactory entitiesFactory)
        {
            OriginData = mapData;
            _entitiesFactory = entitiesFactory;

            InitCounters();
            InitEntities(mapData);

            LevelNumber = new ReactiveProperty<int>(mapData.LevelNumber);
            NumberOfPellets = new ReactiveProperty<int>(mapData.NumberOfPellets);
            NumberOfCollectedPellets = new ReactiveProperty<int>(mapData.NumberOfCollectedPellets);
            //NumberOfCollectedFruits = new ReactiveProperty<int>(mapData.NumberOfCollectedFruits);
        }

        public MapData OriginData { get; }
        public ObservableList<Entity> Entities { get; } = new();

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
                //else if (removedEntity.Type <= EntityType.Chery)
                //{
                //    NumberOfCollectedFruits.Value++;
                //}
            });
        }
    }
}