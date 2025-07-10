using UnityEngine;

namespace MyPacman
{
    public static class Convert
    {
        public static Vector3Int ToTilePosition(Vector2 position)
        {
            int X = (int)position.x;
            int Y = Mathf.Abs((int)(position.y - 1));

            return new Vector3Int(X, Y);
        }
    }
}
