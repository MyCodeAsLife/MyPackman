using R3;

namespace MyPacman
{
    public class PacmanEntity : Entity
    {
        public PacmanEntity(PacmanEntityData pacmanData) : base(pacmanData)
        {
            //AxisPosition = new ReactiveProperty<Vector2>(pacmanData.AxisPosition);
            //AxisPosition.Subscribe(newPosition => pacmanData.AxisPosition = newPosition);

            PositionX = new ReactiveProperty<float>(pacmanData.PositionX);
            PositionX.Subscribe(newPosition => pacmanData.PositionX = newPosition);

            PositionY = new ReactiveProperty<float>(pacmanData.PositionY);
            PositionY.Subscribe(newPosition => pacmanData.PositionY = newPosition);
        }

        public ReactiveProperty<float> PositionX;
        public ReactiveProperty<float> PositionY;
    }
}
