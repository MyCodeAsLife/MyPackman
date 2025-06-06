using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class MapHandler : IMapHandler       // ��������� �� ModelMapHandler � PresenterMapHandler � ������� ����� ���� �������
    {
        private ILevelConfig _currentLevel;     // DI - ? ����� interface
        private Tilemap _wallsTileMap;          // DI - ?
        private Tilemap _pelletsTilemap;        // DI - ?
        private Tile[] _walls;                  // DI - ?

        // For test
        private Vector3Int _currentCellPosition;
        private RuleTile _testTile;

        public MapHandler(Tilemap wallsTileMap, Tilemap pelletsTilemap, Tile[] walls, ILevelConfig level)
        {
            _wallsTileMap = wallsTileMap;
            _pelletsTilemap = pelletsTilemap;
            _walls = walls;
            _currentLevel = level;

            // For test
            _testTile = Resources.Load<RuleTile>("Assets/TestRuleTile");
        }

        // ���������� �����(�� ���� ������� ��������) � ������� ����� ������ ������ tilemap, ������ ������ ������ � ����� �����
        public void ChangeTile(Vector3 position, int objectNumber)
        {
            var handlePosition = ConvertToCellPosition(position);

            _currentLevel.Map[handlePosition.y, handlePosition.x] = objectNumber;                       // �������� ������

            if (objectNumber > 0)
            {
                _wallsTileMap.SetTile(new Vector3Int(handlePosition.x, -handlePosition.y), null);        // �������� Presenter
            }
            else
            {
                _pelletsTilemap.SetTile(new Vector3Int(handlePosition.x, -handlePosition.y), null);
            }
        }

        public bool IsObstacleTile(Vector2 position, string testMessage)      // ����������
        {
            var cellPosition = ConvertToCellPosition(position);
            //Debug.Log(testMessage + cellPosition + $" | pos: {position}");                  // +++++++++++++++++++++++++++++++++++�

            if (_currentCellPosition != cellPosition)
            {
                _currentCellPosition = cellPosition;
                Coroutines.StartRoutine(FleakerTile(cellPosition, _testTile));
            }

            // ��������� ������� �������������� �� 1 ������������ ����������� �������� +
            // �������� ���������� ������� � ��������� ������   +
            // ��������� �� ������������ ������ +
            if (_currentLevel.Map[cellPosition.y, cellPosition.x] > 0)
                return true;

            return false;
        }

        private Vector3Int ConvertToCellPosition(Vector3 position)      // ���������� ��� Vector2?
        {
            int X = (int)position.x;
            int Y = (int)(position.y - 1);

            if (Y < 0)
                Y = Mathf.Abs(Y);
            else
                Y = 0;

            return new Vector3Int(X, Y);
        }

        public bool IsIntersactionTile(int x, int y)       // �������� �� �����������
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

            if (leftX >= 0 && (_currentLevel.Map[y, leftX] == GameConstants.PelletTile ||
                             _currentLevel.Map[y, leftX] == GameConstants.EmptyTile))
            {
                numberOfPaths++;
                vertical++;
            }

            if (rigthX < maxLengthX && (_currentLevel.Map[y, rigthX] == GameConstants.PelletTile ||
                                       _currentLevel.Map[y, rigthX] == GameConstants.EmptyTile))
            {
                numberOfPaths++;
                vertical--;
            }

            if (downY >= 0 && (_currentLevel.Map[downY, x] == GameConstants.PelletTile ||
                               _currentLevel.Map[downY, x] == GameConstants.EmptyTile))
            {
                numberOfPaths++;
                horizontal++;
            }

            if (upY < maxLengthY && (_currentLevel.Map[upY, x] == GameConstants.PelletTile ||
                                        _currentLevel.Map[upY, x] == GameConstants.EmptyTile))
            {
                numberOfPaths++;
                horizontal--;
            }

            return numberOfPaths > 2 || horizontal != 0 || vertical != 0 ? true : false;                                    //Magic
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