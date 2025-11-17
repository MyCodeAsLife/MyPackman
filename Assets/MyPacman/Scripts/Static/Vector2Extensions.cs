using UnityEngine;

namespace MyPacman
{
    public static class Vector2Extensions
    {
        public static float SqrDistance(this Vector2 start, Vector2 end)
        {
            return (end - start).sqrMagnitude;
        }

        public static bool IsEnoughClose(this Vector2 start, Vector2 end, float distance)   // Зачем это? SqrDistance достаточно
        {
            return start.SqrDistance(end) <= distance * distance;
        }

        public static Vector2 Half(this Vector2 value)
        {
            return value * 0.5f;
        }

        public static Vector3 DefineAngle(this Vector2 value)
        {
            Vector3 angle = new Vector3();

            if (value == Vector2.left)
                angle.y = 180;
            else if (value == Vector2.up)
                angle.z = 90;
            else if (value == Vector2.down)
                angle.z = -90;

            return angle;
        }
    }

}
