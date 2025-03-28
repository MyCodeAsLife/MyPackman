using Game.State.GameResources;
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
        public ObservableList<GameResource> Resources { get; } = new();

        // Загружаем\получаем состояния строений
        public GameStateProxy(GameState gameState)
        {
            _gameState = gameState;

            InitMaps();
            InitResources();

            CurrentMapId.Subscribe(newValue => { _gameState.CurrentMapId = newValue; });
        }

        public int CreateEntityId()
        {
            return _gameState.CreateEntityId();
        }

        private void InitMaps()
        {
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
        }

        private void InitResources()
        {
            // Сперва загружаем все состояния в Proxy (перебираем все Entity и на их основе создаем Proxy и связываем их
            _gameState.GameResources.ForEach(resourceData => Resources.Add(new GameResource(resourceData)));

            // Далее регестрируем лямбду которая при создании нового Proxy, будет создавать новый Entyty и записывать в него данные
            Resources.ObserveAdd().Subscribe(element =>
            {
                var addedResource = element.Value;
                _gameState.GameResources.Add(addedResource.Origin);
            });

            // Регестрируем лямбду которая при удалении Proxy, будет искать связанный с ним объект в Entity и удалять его
            Resources.ObserveRemove().Subscribe(element =>
            {
                var removedResource = element.Value;
                var removedResourceData = _gameState.GameResources.FirstOrDefault(b =>
                                          b.GameResourceType == removedResource.GameResourceType);
                _gameState.GameResources.Remove(removedResourceData);
            });

            CurrentMapId.Subscribe(newValue => { _gameState.CurrentMapId = newValue; });
        }
    }
}
