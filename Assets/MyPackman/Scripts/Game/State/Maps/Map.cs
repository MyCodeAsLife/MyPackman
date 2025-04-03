using Game.State.Entities;
using ObservableCollections;
using R3;
using System.Linq;
using UnityEngine;

namespace Game.State.Maps
{
    // Является Proxy
    public class Map
    {
        // Загружаем\получаем состояния строений
        public Map(MapData mapData)
        {
            Origin = mapData;
            // Сперва загружаем все состояния в Proxy (перебираем все Entity и на их основе создаем реактивные свойства и связываем их)
            mapData.Entities.ForEach(entityData => Entities.Add(EntitiesFactory.CreateEntity(entityData)));

            //// Далее регестрируем лямбду которая при создании нового Proxy, будет создавать новый Entyty и записывать в него данные
            Entities.ObserveAdd().Subscribe(element =>
            {
                var addedEntity = element.Value;
                mapData.Entities.Add(addedEntity.Origin);
            });

            //// Регестрируем лямбду которая при удалении Proxy, будет искать связанный с ним объект в Entity и удалять его
            Entities.ObserveRemove().Subscribe(element =>
            {
                var removedEntity = element.Value;
                var removedEntityData = mapData.Entities
                                            .FirstOrDefault(entity => entity.UniqueId == removedEntity.UniqueId);
                mapData.Entities.Remove(removedEntityData);
            });
        }

        // Список эвентов, за которыми можно следить
        public ObservableList<Entity> Entities { get; } = new();
        public MapData Origin { get; }
        public int Id => Origin.Id;
    }
}
