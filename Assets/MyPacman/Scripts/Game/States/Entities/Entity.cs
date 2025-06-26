using R3;
using UnityEngine;

namespace MyPacman
{
    public abstract class Entity
    {
        //public readonly ReactiveProperty<Vector3Int> TilePosition;
        public ReactiveProperty<Vector2> Position;  // Вынести в Entity

        public Entity(EntityData data)
        {
            Origin = data;

            //TilePosition = new ReactiveProperty<Vector3Int>(data.Position);
            //TilePosition.Subscribe(newPosition => data.Position = newPosition);

            // new
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
        //public string ConfigId => Origin.ConfigId;        // ID конфига объека настроек ScriptableObject
        public EntityType Type => Origin.Type;
        public string PrefabPath => Origin.PrefabPath;
    }
}