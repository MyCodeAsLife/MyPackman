using R3;

namespace MyPacman
{
    public class GameState
    {
        private readonly GameStateData _gameStateData;

        public readonly ReactiveProperty<Map> Map;
        public readonly ReactiveProperty<Score> Score;
        public readonly ReactiveProperty<Score> HigthScore;
        public readonly ReactiveProperty<LifePoint> LifePoints;

        public readonly ReactiveProperty<int> CurrentMapId = new();

        public GameState(GameStateData gameStateData, EntitiesFactory entitiesFactory)
        {
            _gameStateData = gameStateData;

            Map = new ReactiveProperty<Map>(new Map(_gameStateData.Map, entitiesFactory));
            Score = new ReactiveProperty<Score>(new Score(_gameStateData.Score));
            HigthScore = new ReactiveProperty<Score>(new Score(_gameStateData.HigthScore));
            LifePoints = new ReactiveProperty<LifePoint>(new LifePoint(_gameStateData.LifePoints));

            CurrentMapId.Subscribe(newValue => { _gameStateData.CurrentMapId = newValue; });

            //InitMap();
            //InitResources();
        }

        //public ObservableList<Map> Maps { get; } = new();
        //public ObservableList<Score> Score { get; } = new();
        //public ObservableList<LifePoint> LifePoints { get; } = new();

        public int CreateEntityId() => _gameStateData.CreateEntityId();

        //private void InitMap()
        //{
        //    _gameStateData.Maps.ForEach(mapStateData => Maps.Add(new Map(mapStateData)));

        //    // При добавлении элемента в Maps текущего класса, добавится элемент в Maps класса GameStateData
        //    Maps.ObserveAdd().Subscribe(collectionAddEvent =>
        //    {
        //        var addedMap = collectionAddEvent.Value;
        //        _gameStateData.Maps.Add(addedMap.Origin);
        //    });

        //    // При удалении элемента из Maps текущего класса, также удалится элемент из Maps класса GameStateData
        //    Maps.ObserveRemove().Subscribe(collectionRemovedEvent =>
        //    {
        //        var removedMap = collectionRemovedEvent.Value;
        //        var removedMapStateData = _gameStateData.Maps.FirstOrDefault(mapStateData =>
        //                                                        mapStateData.Id == removedMap.Id);
        //        _gameStateData.Maps.Remove(removedMapStateData);
        //    });

        //    CurrentMapId.Subscribe(newValue => { _gameStateData.CurrentMapId = newValue; });
        //}

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