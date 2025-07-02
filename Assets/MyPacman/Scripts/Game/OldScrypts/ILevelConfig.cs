namespace MyPacman
{
    public interface ILevelConfig
    {
        public string MapTag { get; }
        public int[,] Map { get; }
    }
}