using ObservableCollections;
using R3;

namespace MyPacman
{
    public class GameState
    {
        public readonly EntitiesFactory EntitiesFactory;

        private readonly GameStateData _gameStateData;
        public readonly ReactiveProperty<Map> Map;
        public readonly ReactiveProperty<int> Score;
        public readonly ReactiveProperty<int> LifePoints;
        public readonly ReactiveProperty<int> NumberOfCollectedFruits;

        public GameState(GameStateData gameStateData)
        {
            _gameStateData = gameStateData;
            EntitiesFactory = new EntitiesFactory(gameStateData.CreateEntityId);

            Map = new ReactiveProperty<Map>(new Map(_gameStateData.Map, EntitiesFactory));
            Score = new ReactiveProperty<int>(_gameStateData.Score);
            LifePoints = new ReactiveProperty<int>(_gameStateData.LifePoints);
            NumberOfCollectedFruits = new ReactiveProperty<int>(_gameStateData.NumberOfCollectedFruits);

            InitMap();
            //InitResources();
        }

        public int CreateEntityId() => _gameStateData.CreateEntityId();

        private void InitMap()
        {
            Map.CurrentValue.Entities.ObserveRemove().Subscribe(collectionRemovedEvent =>
            {
                var removedEntity = collectionRemovedEvent.Value;

                if (removedEntity.Type <= EntityType.Chery)
                    NumberOfCollectedFruits.Value++;
            });

            Score.Subscribe(value => _gameStateData.Score = value);
            LifePoints.Subscribe(value => _gameStateData.LifePoints = value);
            NumberOfCollectedFruits.Subscribe(value => _gameStateData.NumberOfCollectedFruits = value);
        }

        //private void InitResources()
        //{
        //    _gameStateData.Resources.ForEach(resourceData => Resources.Add(new Resource(resourceData)));

        //    Resources.ObserveAdd().Subscribe(collectionAddEvent =>
        //    {
        //        var addedResource = collectionAddEvent.Value;
        //        _gameStateData.Resources.Add(addedResource.Origin);
        //    });

        //    Resources.ObserveRemove().Subscribe(collectionRemovedEvent =>
        //    {
        //        var removedResource = collectionRemovedEvent.Value;
        //        var removedResourceData = _gameStateData.Resources.FirstOrDefault(resourceData =>
        //                                            resourceData.ResourceType == removedResource.ResourceType);
        //        _gameStateData.Resources.Remove(removedResourceData);
        //    });
        //}
    }
}