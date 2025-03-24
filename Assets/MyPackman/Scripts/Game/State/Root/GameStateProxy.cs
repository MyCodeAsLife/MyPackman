using Game.State.Buildings;
using ObservableCollections;
using R3;
using System.Linq;

namespace Game.State.Root
{
    public class GameStateProxy
    {
        // Список эвентов, за которыми можно следить
        public ObservableList<BuildingEntityProxy> Buildings { get; } = new();

        // Загружаем\получаем состояния строений
        public GameStateProxy(GameState gameState)
        {
            // Сперва загружаем все состояния в Proxy (перебираем все Entity и на их основе создаем Proxy и связываем их
            gameState.Buildings.ForEach(buildingOrigin => Buildings.Add(new BuildingEntityProxy(buildingOrigin)));

            // Далее регестрируем лямбду которая при создании нового Proxy, будет создавать новый Entyty и записывать в него данные
            Buildings.ObserveAdd().Subscribe(element =>
            {
                var addedBuildingEntity = element.Value;
                gameState.Buildings.Add(new BuildingEntity()
                {
                    Id = addedBuildingEntity.Id,
                    TypeId = addedBuildingEntity.TypeId,
                    Level = addedBuildingEntity.Level.Value,
                    Position = addedBuildingEntity.Position.Value,
                });
            });

            // Регестрируем лямбду которая при удалении Proxy, будет искать связанный с ним объект в Entity и удалять его
            Buildings.ObserveRemove().Subscribe(element =>
            {
                var removedBuildingEntityProxy = element.Value;
                var removedBuildingEntity = gameState.Buildings
                                            .FirstOrDefault(building => building.Id == removedBuildingEntityProxy.Id);
                gameState.Buildings.Remove(removedBuildingEntity);
            });
        }
    }
}
