namespace Game.State.Entities.Mergeable.Buildings
{
    public class BuildingEntityData : MergeableEntityData
    {
        public double LastClickedTimeMS { get; set; }       // Время последнего клика по зданию, чтобы собрать ресурс
        public bool IsAutoCollectionEnabled { get; set; }   // Возможность проказать автосбор ресурсов (не надо кликать)
    }
}
