using UnityEngine;

namespace MyPacman
{
    public interface IMapHandler
    {
        public int[,] Map { get; }
        //public int GetTile(Vector3Int position);
        public void ChangeTile(Vector3Int position, int objectNumber);     // Поменять на Vector2 ?
        //public bool TryFindPositionByObjectNumber(int number, ref Vector3Int position);
        public bool IsIntersactionTile(int x, int y);
        public bool IsObstacleTile(Vector2 position, string testMessage);
        public void OnPlayerTilesChanged(Vector3Int newTilePosition);
    }
}