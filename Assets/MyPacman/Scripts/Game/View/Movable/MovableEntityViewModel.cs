using R3;
using UnityEngine;

namespace MyPacman
{
    public class MovableEntityViewModel : EntityViewModel
    {
        public readonly ReadOnlyReactiveProperty<bool> IsMoving;
        public readonly ReadOnlyReactiveProperty<Vector2> Direction;

        public MovableEntityViewModel(Entity entity) : base(entity)
        {
            var movableEntity = entity as IMovable;
            IsMoving = movableEntity.IsMoving;
            Direction = movableEntity.Direction;
        }
    }
}
