using ObservableCollections;
using R3;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class MapHandlerService
    {
        private readonly ObservableList<Entity> _entities;
        private readonly Tilemap _obstaclesTileMap;
        private readonly GameState _gameState;

        private readonly Vector2 _fruitSpawnPosition;
        //private readonly FruitSpawner _fruitSpawner;

        // For test
        //private Vector3Int _currentCellPosition;
        //private RuleTile _testTile;

        //public event Action<Edible> EntityEated;

        // 1. создает/удаляет фрукты.
        // 2. контролирует кол-во съедобных сущностей на карте
        // 3. проверки позиций на препятствия?


        public MapHandlerService(GameState gameState, Tilemap obstaclesTileMap, Vector2 fruitSpawnPosition)
        {
            _gameState = gameState;
            _entities = gameState.Map.CurrentValue.Entities;
            _obstaclesTileMap = obstaclesTileMap;
            _fruitSpawnPosition = fruitSpawnPosition;

            gameState.Map.CurrentValue.NumberOfCollectedPellets.Subscribe(OnCollectedPellet);
            // For test
            //_testTile = Resources.Load<RuleTile>("Assets/TestRuleTile");
        }

        public void PlayerTileChanged(Vector2 position)
        {
            var playerTilePosition = ConvertToTilePosition(position);
            // Оптимизировать (выдать всем позицию их тайла?)
            var entity = _entities.FirstOrDefault(entity =>
            {
                var entityPosition = ConvertToTilePosition(entity.Position.Value);

                if (entityPosition.x == playerTilePosition.x && entityPosition.y == playerTilePosition.y)
                    return true;

                return false;
            });

            if (entity != null && entity is Edible)
            {
                if (entity is Ghost)
                {
                    // Проверять режим призрака
                    // если проверка не пройденна return
                }
                else
                {
                    _entities.Remove(entity);
                }
            }
        }

        public bool IsObstacleTile(Vector2 position)
        {
            var tilePosition = ConvertToTilePosition(position);
            var tile = _obstaclesTileMap.GetTile(tilePosition);

            if (tile != null && int.Parse(tile.name) > 0)
                return true;

            return false;
        }

        private Vector3Int ConvertToTilePosition(Vector2 position)      // Вынести в другой класс?
        {
            int X = (int)position.x;
            int Y = Mathf.Abs((int)(position.y - 1));

            return new Vector3Int(X, Y);
        }

        private void OnCollectedPellet(int numberOfCollectedPellets)
        {
            if (numberOfCollectedPellets == GameConstants.CollectedPelletsForFirstFruitSpawn ||
                numberOfCollectedPellets == GameConstants.CollectedPelletsForSecondFruitSpawn)
                SpawnFruit();
        }

        private void SpawnFruit()
        {
            int numFruitType = (int)EntityType.Chery - _gameState.NumberOfCollectedFruits.CurrentValue;

            if (numFruitType < (int)EntityType.Key)
                numFruitType = (int)EntityType.Key;

            var entity = _gameState.EntitiesFactory.CreateEntity(_fruitSpawnPosition, (EntityType)numFruitType);
            _gameState.Map.CurrentValue.Entities.Add(entity);
        }

        //public void OnPlayerTilesChanged(Vector3Int newPlayerTilePosition)      // Обработка содержимого плитки
        //{
        //    //
        //    var tile = _ediblesTilemap.GetTile(new Vector3Int(newPlayerTilePosition.x, -newPlayerTilePosition.y));

        //    if (tile != null)
        //    {
        //        //ChangeTile(newPlayerTilePosition, 0);
        //        int pellet = int.Parse(tile.name);

        //        switch (pellet)
        //        {
        //            case GameConstants.SmallPellet:
        //                Debug.Log(GameConstants.SmallPellet);                         // Magic - SmallPellet RuleTile name
        //                break;

        //            case GameConstants.MediumPellet:
        //                Debug.Log(GameConstants.MediumPellet);                         // Magic - MediumPellet RuleTile name
        //                break;

        //            case GameConstants.LargePellet:
        //                Debug.Log(GameConstants.LargePellet);                         // Magic - LargePellet RuleTile name
        //                break;

        //            default:
        //                throw new System.Exception($"Undefined tile type: {tile.name}");        // Вынести в константы
        //        }

        //    }
        //}

        //public bool IsIntersactionTile(int x, int y)       // Проверка на перекресток
        //{
        //    int numberOfPaths = 0;
        //    int vertical = 0;
        //    int horizontal = 0;
        //    int maxLengthY = _currentLevel.Map.GetLength(0);
        //    int maxLengthX = _currentLevel.Map.GetLength(1);

        //    int leftX = x - 1;
        //    int rigthX = x + 1;
        //    int downY = y - 1;
        //    int upY = y + 1;

        //    if (leftX >= 0 && (_currentLevel.Map[y, leftX] == GameConstants.SmallPellet ||
        //                     _currentLevel.Map[y, leftX] == GameConstants.EmptyTile))
        //    {
        //        numberOfPaths++;
        //        vertical++;
        //    }

        //    if (rigthX < maxLengthX && (_currentLevel.Map[y, rigthX] == GameConstants.SmallPellet ||
        //                               _currentLevel.Map[y, rigthX] == GameConstants.EmptyTile))
        //    {
        //        numberOfPaths++;
        //        vertical--;
        //    }

        //    if (downY >= 0 && (_currentLevel.Map[downY, x] == GameConstants.SmallPellet ||
        //                       _currentLevel.Map[downY, x] == GameConstants.EmptyTile))
        //    {
        //        numberOfPaths++;
        //        horizontal++;
        //    }

        //    if (upY < maxLengthY && (_currentLevel.Map[upY, x] == GameConstants.SmallPellet ||
        //                                _currentLevel.Map[upY, x] == GameConstants.EmptyTile))
        //    {
        //        numberOfPaths++;
        //        horizontal--;
        //    }

        //    return numberOfPaths > 2 || horizontal != 0 || vertical != 0 ? true : false;                                    //Magic
        //}

        //public void ChangeObstacleTile(Vector2 position, int tileNumber)    // Только для конструткора уровня
        //{
        //    var tilePosition = ConvertToTilePosition(position);

        //    _obstaclesTileMap.SetTile(new Vector3Int(tilePosition.x, -tilePosition.y), null);
        //}

        //private void ChangeTile(Vector3Int tilePosition, int objectNumber)
        //{
        //    //var handlePosition = ConvertToCellPosition(position);

        //    _currentLevel.Map[tilePosition.y, tilePosition.x] = objectNumber;                       // Изменяет Модель

        //    if (objectNumber > 0)
        //    {
        //        _obstaclesTileMap.SetTile(new Vector3Int(tilePosition.x, -tilePosition.y), null);        // Изменяет Presenter
        //    }
        //    else
        //    {
        //        _ediblesTilemap.SetTile(new Vector3Int(tilePosition.x, -tilePosition.y), null);
        //    }
        //}

        //private Tile[] LoadObstaclesTiles()
        //{
        //    Tile[] tiles = new Tile[GameConstants.NumberOfWallTiles];

        //    for (int tileName = 0; tileName < GameConstants.NumberOfWallTiles; tileName++)
        //        tiles[tileName] = Resources.Load<Tile>($"{GameConstants.WallTilesFolderPath}{tileName}");

        //    return tiles;
        //}

        //private void LoadEdiblesTiles(string folderPath)
        //{
        //    RuleTile[] ruleTiles = Resources.LoadAll<RuleTile>(folderPath);

        //    for (int i = 0; i < ruleTiles.Length; i++)
        //    {
        //        int tileKey = int.Parse(ruleTiles[i].name);
        //        _edibleTiles[tileKey] = ruleTiles[i];
        //    }
        //}

        //// For test
        //private IEnumerator FleakerTile(Vector3Int position, RuleTile tile)
        //{
        //    float timer = 0.8f;
        //    float delay = 0.2f;

        //    Vector3Int correctPosition = new Vector3Int(position.x, -position.y);

        //    while (timer > 0)
        //    {
        //        _pelletsTilemap.SetTile(correctPosition, tile);
        //        yield return new WaitForSeconds(delay);
        //        _pelletsTilemap.SetTile(correctPosition, null);
        //        timer -= delay;
        //        yield return new WaitForSeconds(delay);
        //        timer -= delay;
        //    }

        //    _pelletsTilemap.SetTile(correctPosition, null);
        //}
    }
}
