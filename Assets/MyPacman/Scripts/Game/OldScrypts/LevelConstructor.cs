using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class LevelConstructor
    {
        private readonly DIContainer _sceneContainer;
        private readonly Tilemap _wallsTileMap;                 // Получать через DI ?
        private readonly Tilemap _pelletsTileMap;               // Получать через DI ?
        private readonly Tilemap _nodesTileMap;                 // Получать через DI ?
        private readonly Tile[] _walls;                         // Получать через DI ?
        private readonly ILevelConfig _level;                   // Получать через DI ?
        private readonly RuleTile[] _pelletsRule;
        private readonly RuleTile[] _nodeRule;
        private readonly IMapHandler _mapHandler;

        private readonly GameState _gameState;

        public LevelConstructor(DIContainer sceneContainer)
        {
            _sceneContainer = sceneContainer;
            _wallsTileMap = sceneContainer.Resolve<Tilemap>(GameConstants.Obstacle);
            _pelletsTileMap = sceneContainer.Resolve<Tilemap>(GameConstants.Pellet);
            _nodesTileMap = sceneContainer.Resolve<Tilemap>(GameConstants.Node);

            _walls = LoadTiles(GameConstants.WallTilesFolderPath, GameConstants.NumberOfWallTiles);
            _pelletsRule = LoadRuleTiles(GameConstants.PelletRuleTilesFolderPath, GameConstants.NumberOfPelletTiles);
            _nodeRule = LoadRuleTiles(GameConstants.NodeRuleTileFolderPath, GameConstants.NumberOfNodeTiles);
            _level = sceneContainer.Resolve<ILevelConfig>();                                                      // Получать от MainMenu? при загрузке сцены
            _sceneContainer.RegisterInstance<IMapHandler>(new MapHandler(_wallsTileMap, _pelletsTileMap, _walls, _level));   // Создание классов вынести в DI?
            _mapHandler = sceneContainer.Resolve<IMapHandler>();

            // Из GameState получить всю инфу о карте.
            // И на ее основе конструировать карту.
            _gameState = sceneContainer.Resolve<GameState>();
            // получить _level
            // получить _pelletsTileMap (переделать в picables и добавить туда все подбираемое)
            // персонажей создавать как отдельные сущности
        }

        public void ConstructLevel()        // Передовать команды в MapHendler, чтобы только он менял Tilemap?
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
                    else if (_level.Map[y, x] == -1)                                                       // Magic
                        SpawnPacman(x, y);
                    else if (_level.Map[y, x] == -4)                                                       // Magic
                    {
                        if (_mapHandler.IsIntersactionTile(x, y))
                        {
                            _nodesTileMap.SetTile(cellPosition, _nodeRule[0]);                     // Magic
                            int chance = Random.Range(0, 100);

                            if (chance < 10)                                                       // Magic
                                _pelletsTileMap.SetTile(cellPosition, _pelletsRule[2]);            // Magic
                            else
                                _pelletsTileMap.SetTile(cellPosition, _pelletsRule[1]);            // Magic
                        }
                        else
                        {
                            _pelletsTileMap.SetTile(cellPosition, _pelletsRule[0]);                // Magic
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

            for (int tileName = 0; tileName < count; tileName++)
                ruleTiles[tileName] = Resources.Load<RuleTile>($"{folderPath}{tileName}");

            return ruleTiles;
        }

        private void SpawnPacman(float x, float y)
        {
            float newX = x * GameConstants.GridCellSize + GameConstants.GridCellSize * GameConstants.Half;
            float newY = -(y * GameConstants.GridCellSize - GameConstants.GridCellSize * GameConstants.Half);
            var newPosition = new Vector3(newX, newY);

            var player = _sceneContainer.Resolve<Pacman>();
            var inputActions = _sceneContainer.Resolve<PlayerInputActions>();
            player.transform.position = newPosition;
            player.transform.rotation = Quaternion.identity;
            player.gameObject.SetActive(true);
            player.Initialize(_mapHandler, inputActions);

            // Не изменять. Понадобится для спавна игрока при смерти
            //_mapHandler.ChangeTile(new Vector3(x, y + 1), GameConstants.EmptyTile);
        }

        private void SpawnPacmanTest(float x, float y)
        {
            float newX = x * GameConstants.GridCellSize + GameConstants.GridCellSize * GameConstants.Half;
            float newY = -(y * GameConstants.GridCellSize - GameConstants.GridCellSize * GameConstants.Half);
            var newPosition = new Vector3(newX, newY);

            var player = _sceneContainer.Resolve<PacmanView>();
            var inputActions = _sceneContainer.Resolve<PlayerInputActions>();
            player.transform.position = newPosition;                            // Позицию получить из GameState
            player.transform.rotation = Quaternion.identity;

            var gameStateService = _sceneContainer.Resolve<IGameStateService>();

            player.Bind(_mapHandler, inputActions);

            player.gameObject.SetActive(true);
        }
    }
}