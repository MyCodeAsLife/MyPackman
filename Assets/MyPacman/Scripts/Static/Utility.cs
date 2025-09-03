using System;
using UnityEngine;

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

        public static bool GreaterThanOrEqual(Vector2 firstPos, Vector2 secondPos, Vector2 direction) // Вынести в Utility или Vector2Extensions
        {
            if (firstPos == secondPos)
                return true;

            if (direction.x != 0)
            {
                if (direction.x > 0)
                    return firstPos.x > secondPos.x;
                else
                    return firstPos.x < secondPos.x;
            }
            else if (direction.y != 0)
            {
                if (direction.y > 0)
                    return firstPos.y > secondPos.y;
                else
                    return firstPos.y < secondPos.y;
            }

            throw new Exception(                                                    // Magic
                $"Unknown error." +
                $"First pos: {firstPos}." +
                $"Second pos: {secondPos}." +
                $"Direction: {direction}");
        }
    }
}
