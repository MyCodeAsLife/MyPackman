namespace MyPacman
{
    public static class Utility
    {
        public static float RepeatInRange(float value, float min, float max)
        {
            if (value < min)
                return max;
            else if (value > max)
                return min;

            return value;
        }
    }
}
