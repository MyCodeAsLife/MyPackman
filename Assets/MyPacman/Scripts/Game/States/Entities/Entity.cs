using R3;
using UnityEngine;

namespace MyPacman
{
    public abstract class Entity
    {
        public ReactiveProperty<Vector2> Position;

        public Entity(EntityData data)
        {
            Origin = data;
            Vector2 nextPosition = new Vector2(data.PositionX, data.PositionY);
            Position = new ReactiveProperty<Vector2>(nextPosition);

            Position.Subscribe(position =>
            {
                data.PositionX = position.x;
                data.PositionY = position.y;
            });
        }

        public EntityData Origin { get; }
        public int UniqueId => Origin.UniqId;
        public EntityType Type => Origin.Type;
        public string PrefabPath => Origin.PrefabPath;      // Используется?
    }
}