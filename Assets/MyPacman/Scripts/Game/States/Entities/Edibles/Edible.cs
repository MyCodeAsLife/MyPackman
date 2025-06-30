namespace MyPacman
{
    public class Edible : Entity
    {
        public readonly EdibleEntityPoints Points;

        public Edible(EdibleData data) : base(data)
        {
            Points = data.Points;
        }
    }
}
