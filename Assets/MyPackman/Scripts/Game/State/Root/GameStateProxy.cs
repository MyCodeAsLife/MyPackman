using Game.State.Buildings;
using ObservableCollections;
using R3;
using System.Linq;

namespace Game.State.Root
{
    public class GameStateProxy
    {
        private readonly GameState _gameState;

        // Список эвентов, за которыми можно следить
        public ObservableList<BuildingEntityProxy> Buildings { get; } = new();

        // Загружаем\получаем состояния строений
        public GameStateProxy(GameState gameState)
        {
            _gameState = gameState;
            // Сперва загружаем все состояния в Proxy (перебираем все Entity и на их основе создаем Proxy и связываем их
            _gameState.Buildings.ForEach(buildingOrigin => Buildings.Add(new BuildingEntityProxy(buildingOrigin)));

            // Далее регестрируем лямбду которая при создании нового Proxy, будет создавать новый Entyty и записывать в него данные
            Buildings.ObserveAdd().Subscribe(element =>
            {
                var addedBuildingEntity = element.Value;
                _gameState.Buildings.Add(addedBuildingEntity.Origin);
            });

            // Регестрируем лямбду которая при удалении Proxy, будет искать связанный с ним объект в Entity и удалять его
            Buildings.ObserveRemove().Subscribe(element =>
            {
                var removedBuildingEntityProxy = element.Value;
                var removedBuildingEntity = _gameState.Buildings
                                            .FirstOrDefault(building => building.Id == removedBuildingEntityProxy.Id);
                _gameState.Buildings.Remove(removedBuildingEntity);
            });
        }

        public int GetEntityId()
        {
            return _gameState.GlobalEntityId++;
        }
    }
}
