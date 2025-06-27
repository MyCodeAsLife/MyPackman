using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class LevelConstructor
    {
        private readonly DIContainer _sceneContainer;
        private readonly Tilemap _wallsTileMap;                 // �������� ����� DI ?
        private readonly Tilemap _pickablesTileMap;               // �������� ����� DI ?
        //private readonly Tilemap _FruitsTileMap;                 // �������� ����� DI ?
        private readonly Tile[] _walls;                         // �������� ����� DI ?
        private readonly RuleTile[] _pelletsRuleTiles;
        //private readonly RuleTile[] _FruitsRuleTiles;
        private readonly ILevelConfig _level;                   // �������� ����� DI ?
        private readonly IMapHandler _mapHandler;

        private readonly GameState _gameState;

        public LevelConstructor(DIContainer sceneContainer)
        {
            _sceneContainer = sceneContainer;
            _wallsTileMap = sceneContainer.Resolve<Tilemap>(GameConstants.Obstacle);
            _pickablesTileMap = sceneContainer.Resolve<Tilemap>(GameConstants.Pellet);
            //_FruitsTileMap = sceneContainer.Resolve<Tilemap>(GameConstants.Fruit);

            _walls = LoadTiles(GameConstants.WallTilesFolderPath, GameConstants.NumberOfWallTiles);
            _pelletsRuleTiles = LoadRuleTiles(GameConstants.PelletRuleTilesFolderPath, GameConstants.NumberOfPelletTiles);
            //_FruitsRuleTiles = LoadRuleTiles(GameConstants.FruitRuleTileFolderPath, GameConstants.NumberOfFruitTiles);
            _level = sceneContainer.Resolve<ILevelConfig>();                                                      // �������� �� MainMenu? ��� �������� �����
            sceneContainer.RegisterInstance<IMapHandler>(new MapHandler(_wallsTileMap, _pickablesTileMap, _walls, _level));   // �������� ������� ������� � DI?
            _mapHandler = sceneContainer.Resolve<IMapHandler>();

            // �� GameState �������� ��� ���� � �����.
            // � �� �� ������ �������������� �����.
            _gameState = sceneContainer.Resolve<IGameStateService>().GameState;
            // �������� _level
            // ���������� ��������� ��� ��������� ��������
        }

        // ��� ��������������� ������:
        // 1 - ��������� �� ������� Entities � gameState
        // 2 - ���� �� ����� �� �� "������������" ����� ����� ������ �����������
        // 3 - ���� ����� �� ����� ��� �� "������������" ����� � ������� ��������
        public void ConstructLevel()        // ���������� ������� � MapHendler, ����� ������ �� ����� Tilemap?
        {
            _wallsTileMap.ClearAllTiles();

            for (int y = 0; y < _level.Map.GetLength(0); y++)
            {
                for (int x = 0; x < _level.Map.GetLength(1); x++)
                {
                    var cellPosition = new Vector3Int(x, -y);

                    if (_level.Map[y, x] > 0)                                                              // Magic
                        _wallsTileMap.SetTile(cellPosition, _walls[_level.Map[y, x]]);
                    else if (_level.Map[y, x] == GameConstants.EmptyTile)
                        _wallsTileMap.SetTile(cellPosition, null);
                    else if (_level.Map[y, x] == GameConstants.PacmanSpawn)
                        SpawnPacmanTest(x, y);
                    else if (_level.Map[y, x] == GameConstants.SmallPellet)
                    {
                        if (_mapHandler.IsIntersactionTile(x, y))
                        {
                            //_FruitsTileMap.SetTile(cellPosition, _FruitRule[0]);                     // Magic
                            int chance = Random.Range(0, 100);

                            if (chance < 10)                                                       // Magic
                                _pickablesTileMap.SetTile(cellPosition, _pelletsRuleTiles[2]);            // Magic
                            else
                                _pickablesTileMap.SetTile(cellPosition, _pelletsRuleTiles[1]);            // Magic
                        }
                        else
                        {
                            _pickablesTileMap.SetTile(cellPosition, _pelletsRuleTiles[0]);                // Magic
                        }
                    }
                }
            }
        }

        private Tile[] LoadTiles(string folderPath, int count)
        {
            Tile[] tiles = new Tile[count];

            for (int tileName = 0; tileName < count; tileName++)
                tiles[tileName] = Resources.Load<Tile>($"{folderPath}{tileName}");

            return tiles;
        }

        private RuleTile[] LoadRuleTiles(string folderPath, int count)
        {
            RuleTile[] ruleTiles = new RuleTile[count];

            //for (int tileName = 0; tileName < count; tileName++)
            //    ruleTiles[tileName] = Resources.Load<RuleTile>($"{folderPath}{tileName}");

            ruleTiles = Resources.LoadAll<RuleTile>(folderPath);

            return ruleTiles;
        }

        private void SpawnPacman(float x, float y)
        {
            float newX = x * GameConstants.GridCellSize + GameConstants.GridCellSize * GameConstants.Half;
            float newY = -(y * GameConstants.GridCellSize - GameConstants.GridCellSize * GameConstants.Half);
            var newPosition = new Vector3(newX, newY);

            var player = _sceneContainer.Resolve<OldPacman>();
            var inputActions = _sceneContainer.Resolve<PlayerInputActions>();
            player.transform.position = newPosition;
            //player.transform.rotation = Quaternion.identity;
            //player.gameObject.SetActive(true);
            var timeService = _sceneContainer.Resolve<TimeService>();
            player.Initialize(_mapHandler, inputActions, timeService);

            // �� ��������. ����������� ��� ������ ������ ��� ������
            //_mapHandler.ChangeTile(new Vector3(x, y + 1), GameConstants.EmptyTile);
        }

        private void SpawnPacmanTest(float x, float y)
        {
            var Pacman = _gameState.Map.Value.Entities.First(entity => entity.Type == EntityType.Pacman);
            var gameStateService = _sceneContainer.Resolve<IGameStateService>();

            var player = _sceneContainer.Resolve<OldPacmanView>();
            var inputActions = _sceneContainer.Resolve<PlayerInputActions>();
            var timeService = _sceneContainer.Resolve<TimeService>();

            float newX = 0;
            float newY = 0;

            if (Pacman.Position.Value == Vector2.zero)     // ������� ���������� �� ����� ��������?
            {
                newX = x * GameConstants.GridCellSize + GameConstants.GridCellSize * GameConstants.Half;
                newY = -(y * GameConstants.GridCellSize - GameConstants.GridCellSize * GameConstants.Half);
            }
            else
            {
                var position = Pacman.Position.Value;
                newX = position.x * GameConstants.GridCellSize + GameConstants.GridCellSize * GameConstants.Half;
                newY = -(position.y * GameConstants.GridCellSize - GameConstants.GridCellSize * GameConstants.Half);
            }

            player.transform.rotation = Quaternion.identity;
            player.transform.position = new Vector3(newX, newY);

            player.Bind(Pacman as Pacman, inputActions, gameStateService, _mapHandler, timeService);

            player.gameObject.SetActive(true);
        }
    }
}