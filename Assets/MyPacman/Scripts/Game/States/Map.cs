using ObservableCollections;
using R3;
using System.Linq;

namespace MyPacman
{
    public class Map
    {
        private EntitiesFactory _entitiesFactory = new();       // Или вынести регистрацию в DI?

        public Map(MapData mapData)
        {
            OriginData = mapData;
            mapData.Entities.ForEach(entityData => Entities.Add(_entitiesFactory.CreateEntity(entityData)));

            // При добавлении элемента в Entities текущего класса, добавится элемент в Entities класса GameState
            Entities.ObserveAdd().Subscribe(collectionAddEvent =>
            {
                var addedEntity = collectionAddEvent.Value;
                mapData.Entities.Add(addedEntity.Origin);
            });

            // При удалении элемента из Entities текущего класса, также удалится элемент из Entities класса GameState
            Entities.ObserveRemove().Subscribe(collectionRemovedEvent =>
            {
                var removedEntity = collectionRemovedEvent.Value;
                var removedEntityData = mapData.Entities.FirstOrDefault(entityData =>
                                                    entityData.UniqId == removedEntity.UniqueId);
                mapData.Entities.Remove(removedEntityData);
            });
        }

        public int Id => OriginData.Id;
        public MapData OriginData { get; }
        public ObservableList<Entity> Entities { get; } = new();
    }
}