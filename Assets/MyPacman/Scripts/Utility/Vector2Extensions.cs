using UnityEngine;

namespace MyPacman
{
    public static class Vector2Extensions
    {
        public static float SqrDistance(this Vector2 start, Vector2 end)
        {
            return (end - start).sqrMagnitude;
        }

        public static bool IsEnoughClose(this Vector2 start, Vector2 end, float distance)
        {
            return start.SqrDistance(end) <= distance * distance;
        }

        public static Vector2 Half(this Vector2 value)
        {
            return value / 2f;
        }
    }

}
