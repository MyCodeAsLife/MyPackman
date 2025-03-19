using UnityEngine;

namespace Assets.MyPackman.Presenter
{
    public interface IMapHandler
    {
        public int Tile(Vector3Int position);
        public void ChangeTile(Vector3Int position, int objectNumber);
        public bool TryFindPositionByObjectNumber(int number, ref Vector3Int position);
    }
}
