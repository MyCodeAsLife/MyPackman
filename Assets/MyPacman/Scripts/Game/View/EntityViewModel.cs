using R3;
using UnityEngine;

namespace MyPacman
{
    public abstract class EntityViewModel
    {
        public readonly ReadOnlyReactiveProperty<Vector2> Position;
        public readonly int EntityId;

        protected readonly Entity Entity;

        public EntityViewModel(Entity entity)
        {
            Entity = entity;
            Position = entity.Position;
            EntityId = entity.UniqueId;
        }

        public string PrefabPath => Entity.PrefabPath;  // Используется?
    }
}
