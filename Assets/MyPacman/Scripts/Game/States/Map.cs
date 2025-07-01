using ObservableCollections;
using R3;
using System.Linq;

namespace MyPacman
{
    public class Map
    {
        public readonly ReactiveProperty<int> LevelNumber;
        public readonly ReactiveProperty<int> NumberOfFruits;
        public readonly ReactiveProperty<int> NumberOfPellets;
        public readonly ReactiveProperty<int> NumberOfCollectedPellets;

        private readonly EntitiesFactory _entitiesFactory;

        public Map(MapData mapData, EntitiesFactory entitiesFactory)
        {
            OriginData = mapData;
            _entitiesFactory = entitiesFactory;

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

            LevelNumber = new ReactiveProperty<int>(mapData.LevelNumber);
            NumberOfFruits = new ReactiveProperty<int>(mapData.NumberOfFruits);
            NumberOfPellets = new ReactiveProperty<int>(mapData.NumberOfPellets);
            NumberOfCollectedPellets = new ReactiveProperty<int>(mapData.NumberOfCollectedPellets);
        }

        public MapData OriginData { get; }
        public ObservableList<Entity> Entities { get; } = new();
    }
}