using Game.State.Maps;
using ObservableCollections;
using R3;
using System.Linq;

namespace Game.State.Root
{
    public class GameStateProxy
    {
        private readonly GameState _gameState;
        public readonly ReactiveProperty<int> CurrentMapId = new();

        // Список эвентов, за которыми можно следить
        public ObservableList<Map> Maps { get; } = new();

        // Загружаем\получаем состояния строений
        public GameStateProxy(GameState gameState)
        {
            _gameState = gameState;
            // Сперва загружаем все состояния в Proxy (перебираем все Entity и на их основе создаем Proxy и связываем их
            _gameState.Maps.ForEach(mapOrigin => Maps.Add(new Map(mapOrigin)));

            // Далее регестрируем лямбду которая при создании нового Proxy, будет создавать новый Entyty и записывать в него данные
            Maps.ObserveAdd().Subscribe(element =>
            {
                var addedMap = element.Value;
                _gameState.Maps.Add(addedMap.Origin);
            });

            // Регестрируем лямбду которая при удалении Proxy, будет искать связанный с ним объект в Entity и удалять его
            Maps.ObserveRemove().Subscribe(element =>
            {
                var removedMap = element.Value;
                var removedMapState = _gameState.Maps.FirstOrDefault(b => b.Id == removedMap.Id);
                _gameState.Maps.Remove(removedMapState);
            });

            CurrentMapId.Subscribe(newValue => { gameState.CurrentMapId = newValue; });
        }

        public int CreateEntityId()
        {
            return _gameState.CreateEntityId();
        }
    }
}
