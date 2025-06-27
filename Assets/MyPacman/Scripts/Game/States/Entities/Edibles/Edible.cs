namespace MyPacman
{
    public class Edible : Entity
    {
        public readonly EntityPoints Points;

        public Edible(EdibleData data) : base(data)
        {
            Points = data.Points;
        }
    }
}
