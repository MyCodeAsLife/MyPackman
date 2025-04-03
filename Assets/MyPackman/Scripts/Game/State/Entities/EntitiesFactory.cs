using Game.State.Entities.Mergeable.Buildings;
using Game.State.Entities.Mergeable.ResourcesEntities;
using System;

namespace Game.State.Entities
{
    public static class EntitiesFactory
    {
        public static Entity CreateEntity(EntityData entityData)
        {
            switch (entityData.Type)
            {
                case EntityType.Building:
                    return new BuildingEntity(entityData as BuildingEntityData);    // Приведение типа дорогая операция, но здесь она будет делаться редко

                case EntityType.Resource:
                    return new ResourceEntity(entityData as ResourceEntityData);

                default:
                    throw new Exception("Unsuported entity type" + entityData.Type);
            }
        }
    }
}
