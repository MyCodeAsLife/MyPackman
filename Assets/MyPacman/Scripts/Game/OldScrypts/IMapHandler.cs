using UnityEngine;

namespace MyPacman
{
    public interface IMapHandler
    {
        //public int GetTile(Vector3Int position);
        public void ChangeTile(Vector3 position, int objectNumber);     // �������� �� Vector2 ?
        //public bool TryFindPositionByObjectNumber(int number, ref Vector3Int position);
        public bool IsIntersactionTile(int x, int y);
        public bool IsObstacleTile(Vector2 position, string testMessage);
    }
}