using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class MapHandler : IMapHandler       // Разделить на ModelMapHandler и PresenterMapHandler и связать через шину событий
    {
        private ILevelConfig _currentLevel;     // DI - ? через interface
        private Tilemap _wallsTileMap;          // DI - ?
        private Tilemap _pelletsTilemap;        // DI - ?
        //private Tile[] _walls;                  // DI - ?

        // For test
        private Vector3Int _currentCellPosition;
        private RuleTile _testTile;


        public MapHandler(Tilemap wallsTileMap, Tilemap pelletsTilemap, Tile[] walls, ILevelConfig level)
        {
            _wallsTileMap = wallsTileMap;
            _pelletsTilemap = pelletsTilemap;
            //_walls = walls;
            _currentLevel = level;

            // For test
            _testTile = Resources.Load<RuleTile>("Assets/TestRuleTile");
        }

        public int[,] Map => _currentLevel.Map;

        public void ChangeTile(Vector3Int tilePosition, int objectNumber)
        {
            //var handlePosition = ConvertToCellPosition(position);

            _currentLevel.Map[tilePosition.y, tilePosition.x] = objectNumber;                       // Изменяет Модель

            if (objectNumber > 0)
            {
                _wallsTileMap.SetTile(new Vector3Int(tilePosition.x, -tilePosition.y), null);        // Изменяет Presenter
            }
            else
            {
                _pelletsTilemap.SetTile(new Vector3Int(tilePosition.x, -tilePosition.y), null);
            }
        }

        // В данный момент не используется
        public bool IsObstacleTile(Vector2 position, string testMessage)      // Незакончен
        {

            var cellPosition = ConvertToTilePosition(position);

            if (_currentCellPosition != cellPosition)
            {
                _currentCellPosition = cellPosition;
                Coroutines.StartRoutine(FleakerTile(cellPosition, _testTile));
            }

            if (_currentLevel.Map.GetLength(0) <= cellPosition.y ||
                cellPosition.y <= 0 ||
                _currentLevel.Map.GetLength(1) <= cellPosition.x ||
                cellPosition.x <= 0)
                return false;

            if (_currentLevel.Map[cellPosition.y, cellPosition.x] > 0)
                return true;

            return false;
        }

        public Vector3Int ConvertToTilePosition(Vector2 position)      // Вынести в другой класс?
        {
            int X = (int)position.x;
            int Y = Mathf.Abs((int)(position.y - 1));

            return new Vector3Int(X, Y);
        }

        // Выпилить!
        public bool IsIntersactionTile(int x, int y)       // Проверка на перекресток
        {
            int numberOfPaths = 0;
            int vertical = 0;
            int horizontal = 0;
            int maxLengthY = _currentLevel.Map.GetLength(0);
            int maxLengthX = _currentLevel.Map.GetLength(1);

            int leftX = x - 1;
            int rigthX = x + 1;
            int downY = y - 1;
            int upY = y + 1;

            if (leftX >= 0 && (_currentLevel.Map[y, leftX] == (int)EntityType.SmallPellet ||
                             _currentLevel.Map[y, leftX] == GameConstants.EmptyTile))
            {
                numberOfPaths++;
                vertical++;
            }

            if (rigthX < maxLengthX && (_currentLevel.Map[y, rigthX] == (int)EntityType.SmallPellet ||
                                       _currentLevel.Map[y, rigthX] == GameConstants.EmptyTile))
            {
                numberOfPaths++;
                vertical--;
            }

            if (downY >= 0 && (_currentLevel.Map[downY, x] == (int)EntityType.SmallPellet ||
                               _currentLevel.Map[downY, x] == GameConstants.EmptyTile))
            {
                numberOfPaths++;
                horizontal++;
            }

            if (upY < maxLengthY && (_currentLevel.Map[upY, x] == (int)EntityType.SmallPellet ||
                                        _currentLevel.Map[upY, x] == GameConstants.EmptyTile))
            {
                numberOfPaths++;
                horizontal--;
            }

            return numberOfPaths > 2 || horizontal != 0 || vertical != 0 ? true : false;                                    //Magic
        }

        public void OnPlayerTilesChanged(Vector3Int newPlayerTilePosition)      // Обработка содержимого плитки
        {
            //
            var tile = _pelletsTilemap.GetTile(new Vector3Int(newPlayerTilePosition.x, -newPlayerTilePosition.y));

            if (tile != null)
            {
                ChangeTile(newPlayerTilePosition, 0);
                int pellet = int.Parse(tile.name);

                switch (pellet)
                {
                    case (int)EntityType.SmallPellet:
                        Debug.Log(EntityType.SmallPellet);                         // Magic - SmallPellet RuleTile name
                        break;

                    case (int)EntityType.MediumPellet:
                        Debug.Log(EntityType.MediumPellet);                         // Magic - MediumPellet RuleTile name
                        break;

                    case (int)EntityType.LargePellet:
                        Debug.Log(EntityType.LargePellet);                         // Magic - LargePellet RuleTile name
                        break;

                    default:
                        throw new System.Exception($"Undefined tile type: {tile.name}");        // Вынести в константы
                }

            }
        }

        // For test
        private IEnumerator FleakerTile(Vector3Int position, RuleTile tile)
        {
            float timer = 0.8f;
            float delay = 0.2f;

            Vector3Int correctPosition = new Vector3Int(position.x, -position.y);

            while (timer > 0)
            {
                _pelletsTilemap.SetTile(correctPosition, tile);
                yield return new WaitForSeconds(delay);
                _pelletsTilemap.SetTile(correctPosition, null);
                timer -= delay;
                yield return new WaitForSeconds(delay);
                timer -= delay;
            }

            _pelletsTilemap.SetTile(correctPosition, null);
        }
    }
}