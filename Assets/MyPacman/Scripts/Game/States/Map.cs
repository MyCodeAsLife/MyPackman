using ObservableCollections;
using R3;
using System.Linq;

namespace MyPacman
{
    public class Map    // Переименовать в MapState
    {
        public Map(MapData mapState)
        {
            Origin = mapState;
            mapState.Entities.ForEach(entityData => Entities.Add(EntitiesFactory.CreateEntity(entityData)));

            // При добавлении элемента в Entities текущего класса, добавится элемент в Entities класса GameState
            Entities.ObserveAdd().Subscribe(collectionAddEvent =>
            {
                var addedEntity = collectionAddEvent.Value;
                mapState.Entities.Add(addedEntity.Origin);
            });

            // При удалении элемента из Entities текущего класса, также удалится элемент из Entities класса GameState
            Entities.ObserveRemove().Subscribe(collectionRemovedEvent =>
            {
                var removedEntity = collectionRemovedEvent.Value;
                var removedEntityData = mapState.Entities.FirstOrDefault(entityData =>
                                                    entityData.UniqId == removedEntity.UniqueId);
                mapState.Entities.Remove(removedEntityData);
            });
        }

        public int Id => Origin.Id;
        public MapData Origin { get; }
        public ObservableList<Entity> Entities { get; } = new();
    }
}