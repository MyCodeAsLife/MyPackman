using Game.State.Buildings;
using ObservableCollections;
using R3;
using System.Linq;

namespace Game.State.Maps
{
    // Является Proxy
    public class Map
    {
        // Загружаем\получаем состояния строений
        public Map(MapState mapState)
        {
            Origin = mapState;
            // Сперва загружаем все состояния в Proxy (перебираем все Entity и на их основе создаем Proxy и связываем их
            mapState.Buildings.ForEach(buildingOrigin => Buildings.Add(new BuildingEntityProxy(buildingOrigin)));

            // Далее регестрируем лямбду которая при создании нового Proxy, будет создавать новый Entyty и записывать в него данные
            Buildings.ObserveAdd().Subscribe(element =>
            {
                var addedBuildingEntity = element.Value;
                mapState.Buildings.Add(addedBuildingEntity.Origin);
            });

            // Регестрируем лямбду которая при удалении Proxy, будет искать связанный с ним объект в Entity и удалять его
            Buildings.ObserveRemove().Subscribe(element =>
            {
                var removedBuildingEntityProxy = element.Value;
                var removedBuildingEntity = mapState.Buildings
                                            .FirstOrDefault(building => building.Id == removedBuildingEntityProxy.Id);
                mapState.Buildings.Remove(removedBuildingEntity);
            });
        }

        // Список эвентов, за которыми можно следить
        public ObservableList<BuildingEntityProxy> Buildings { get; } = new();
        public MapState Origin { get; }
        public int Id => Origin.Id;
    }
}
