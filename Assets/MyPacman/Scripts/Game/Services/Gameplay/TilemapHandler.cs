using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class TilemapHandler
    {
        private readonly Tilemap _obstaclesTileMap;
        private readonly ILevelConfig _levelConfig;

        public TilemapHandler(Tilemap obstaclesTileMap, ILevelConfig levelConfig)
        {
            _obstaclesTileMap = obstaclesTileMap;
            _levelConfig = levelConfig;
        }

        public bool CheckTileForObstacle(Vector2 position)
        {
            var tilePosition = Convert.ToTilePosition(position);
            return CheckTileForObstacle(tilePosition);
        }

        public List<Vector2> GetDirectionsWithoutObstacles(Vector2 position)
        {
            var tilePosition = Convert.ToTilePosition(position);
            List<Vector2> directions = new();

            if (CheckTileForObstacle(tilePosition + Vector3Int.left) == false)
                directions.Add(Vector2.left);

            if (CheckTileForObstacle(tilePosition + Vector3Int.right) == false)
                directions.Add(Vector2.right);

            if (CheckTileForObstacle(tilePosition + Vector3Int.up) == false)
                directions.Add(Vector2.up);

            if (CheckTileForObstacle(tilePosition + Vector3Int.down) == false)
                directions.Add(Vector2.down);

            return directions;
        }

        public List<Vector2> GetDirectionsWithoutWalls(Vector2 position)
        {
            var tilePosition = Convert.ToTilePosition(position);
            List<Vector2> directions = GetDirectionsWithoutObstacles(position);

            if (CheckTileForType(tilePosition + Vector3Int.left, GameConstants.GateTile))
                directions.Add(Vector2.left);

            if (CheckTileForType(tilePosition + Vector3Int.right, GameConstants.GateTile))
                directions.Add(Vector2.right);

            if (CheckTileForType(tilePosition + Vector3Int.up, GameConstants.GateTile))
                directions.Add(Vector2.up);

            if (CheckTileForType(tilePosition + Vector3Int.down, GameConstants.GateTile))
                directions.Add(Vector2.down);

            return directions;
        }

        public bool IsCenterTail(Vector2 position)
        {
            var valueX = position.x - Mathf.Floor(position.x);
            var valueY = position.y - Mathf.Floor(position.y);

            if (Mathf.Approximately(valueX, GameConstants.Half) && Mathf.Approximately(valueY, GameConstants.Half))
                return true;

            return false;
        }

        public List<Vector2> GetTilePositions(int numTile)
        {
            List<Vector2> result = new List<Vector2>();

            for (int y = 0; y < _levelConfig.Map.GetLength(0); y++)
                for (int x = 0; x < _levelConfig.Map.GetLength(1); x++)
                    if (_levelConfig.Map[y, x] == numTile)
                        result.Add(new Vector2(x + GameConstants.Half, -y - GameConstants.Half));

            return result;
        }

        public bool CheckTile(Vector2 position, int numTile)
        {
            var tilePos = Convert.ToTilePosition(position);
            var tile = _obstaclesTileMap.GetTile(tilePos);

            if (tile != null)
            {
                var num = int.Parse(tile.name);
                return num == numTile;
            }

            return false;
        }

        private bool CheckTileForObstacle(Vector3Int tilePos)
        {
            var tile = _obstaclesTileMap.GetTile(tilePos);
            return tile != null;
        }

        private bool CheckTileForType(Vector3Int tilePos, int tileType)
        {
            var tile = _obstaclesTileMap.GetTile(tilePos);

            if (tile != null)
            {
                int numType = int.Parse(tile.name);
                return numType == tileType;
            }

            return false;
        }
    }
}