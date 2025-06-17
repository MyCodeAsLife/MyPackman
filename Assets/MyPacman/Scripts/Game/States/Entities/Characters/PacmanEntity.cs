using R3;
using UnityEngine;

namespace MyPacman
{
    public class PacmanEntity : Entity
    {
        public PacmanEntity(PacmanEntityData pacmanData) : base(pacmanData)
        {
            AxisPosition = new ReactiveProperty<Vector2>(pacmanData.AxisPosition);
            AxisPosition.Subscribe(newPosition => pacmanData.AxisPosition = newPosition);
        }

        public ReactiveProperty<Vector2> AxisPosition;
    }
}
