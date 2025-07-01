﻿using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyPacman
{
    public class LevelCreator
    {
        //private readonly DIContainer _sceneContainer;
        //private readonly Tilemap _ediblesTileMap;               // Получать через DI ?
        //private readonly Tilemap _FruitsTileMap;                 // Получать через DI ?
        private readonly Tilemap _obstacleTileMap = new();                 // Получать через DI ?
        private readonly Tile[] _obstacleTiles;                         // Получать через DI ?
        //private readonly RuleTile[] _pelletsRuleTiles;
        //private readonly RuleTile[] _FruitsRuleTiles;
        private readonly ILevelConfig _level;                   // Получать через DI ?
        //private readonly MapHandlerService _mapHandler;

        private readonly GameState _gameState;
        private readonly EntitiesFactory _entitiesFactory;          // Получать через DI
        //private readonly EntitiesDataFactory _entitiesDataFactory = new();  // Получать через DI
        private readonly bool _isLoaded = false;

        public LevelCreator(DIContainer sceneContainer)
        {
            //_sceneContainer = sceneContainer;
            //_obstacleTileMap = sceneContainer.Resolve<Tilemap>(GameConstants.Obstacle);         // Нужно ли его регестрировать в контейнере?
            //_ediblesTileMap = sceneContainer.Resolve<Tilemap>(GameConstants.Pellet);
            //_FruitsTileMap = sceneContainer.Resolve<Tilemap>(GameConstants.Fruit);

            _obstacleTiles = LoadTiles(GameConstants.WallTilesFolderPath, GameConstants.NumberOfWallTiles);
            //_pelletsRuleTiles = LoadRuleTiles(GameConstants.PelletRuleTilesFolderPath, GameConstants.NumberOfPelletTiles);
            //_FruitsRuleTiles = LoadRuleTiles(GameConstants.FruitRuleTileFolderPath, GameConstants.NumberOfFruitTiles);
            var gameStateService = sceneContainer.Resolve<IGameStateService>();
            _level = sceneContainer.Resolve<ILevelConfig>();                                     // Получать от MainMenu? при загрузке сцены
            _gameState = gameStateService.GameState;
            _entitiesFactory = new EntitiesFactory(_gameState);                                  // Получать через DI
            _isLoaded = gameStateService.GameStateIsLoaded;
            var mapHandler = new MapHandlerService(_gameState.Map, _obstacleTileMap);
            sceneContainer.RegisterInstance(mapHandler);   // Создание классов вынести в DI?
            //_mapHandler = sceneContainer.Resolve<MapHandlerService>();

            // Из GameState получить всю инфу о карте.
            // И на ее основе конструировать карту.
            // получить _level
            // персонажей создавать как отдельные сущности
        }

        // При конструировании уровня:
        // 1 - Проверить на пустоту Entities в gameState
        // 2 - Если не пусто то из "оригинальной" карты брать только препятствия
        // 3 - Если пусто то берем все из "оригинальной" карты и создаем сущности
        public void ConstructLevel()        // Передовать команды в MapHendler, чтобы только он менял Tilemap?
        {
            _obstacleTileMap.ClearAllTiles();

            for (int y = 0; y < _level.Map.GetLength(0); y++)
            {
                for (int x = 0; x < _level.Map.GetLength(1); x++)
                {
                    //var cellPosition = new Vector3Int(x, -y);
                    int numTile = _level.Map[y, x] - 1;
                    if (numTile >= 0)                                                              // Magic
                        //_obstacleTileMap.SetTile(cellPosition, _obstacleTiles[numTile]);
                        CreateObstacle(numTile, x, y);
                    //else if (numTile == GameConstants.EmptyTile)
                    //    _obstacleTileMap.SetTile(cellPosition, null);
                    else if (_isLoaded == false)
                        //SpawnPacmanTest(x, y);
                        CreateEntity(x, y, (EntityType)numTile);
                    //else if (_level.Map[y, x] == GameConstants.SmallPellet)
                    //{
                    //    if (_mapHandler.IsIntersactionTile(x, y))
                    //    {
                    //        //_FruitsTileMap.SetTile(cellPosition, _FruitRule[0]);                     // Magic
                    //        int chance = Random.Range(0, 100);

                    //        if (chance < 10)                                                       // Magic
                    //            _ediblesTileMap.SetTile(cellPosition, _pelletsRuleTiles[2]);            // Magic
                    //        else
                    //            _ediblesTileMap.SetTile(cellPosition, _pelletsRuleTiles[1]);            // Magic
                    //    }
                    //    else
                    //    {
                    //        _ediblesTileMap.SetTile(cellPosition, _pelletsRuleTiles[0]);                // Magic
                    //    }
                    //}
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

        private void CreateObstacle(int x, int y, int numTile)
        {
            var tilePos = new Vector3Int(x, -y);
            Tile tile = numTile > 0 ? _obstacleTiles[numTile] : null;
            _obstacleTileMap.SetTile(tilePos, tile);
        }

        private void CreateEntity(int x, int y, EntityType entityType)
        {
            var entity = _gameState.Map.Value.Entities.FirstOrDefault(entity => entity.Type == entityType);

            if (entity == null)
            {
                //var entityData = _entitiesDataFactory.CreateEntityData(_gameState, new Vector2(x, y), entityType);
                entity = _entitiesFactory.CreateEntity(new Vector2(x, y), entityType);
                _gameState.Map.Value.Entities.Add(entity);
            }
        }

        //private RuleTile[] LoadRuleTiles(string folderPath, int count)
        //{
        //    RuleTile[] ruleTiles = new RuleTile[count];

        //    //for (int tileName = 0; tileName < count; tileName++)
        //    //    ruleTiles[tileName] = Resources.Load<RuleTile>($"{folderPath}{tileName}");

        //    ruleTiles = Resources.LoadAll<RuleTile>(folderPath);

        //    return ruleTiles;
        //}

        //private void SpawnPacman(float x, float y)
        //{
        //    float newX = x * GameConstants.GridCellSize + GameConstants.GridCellSize * GameConstants.Half;
        //    float newY = -(y * GameConstants.GridCellSize - GameConstants.GridCellSize * GameConstants.Half);
        //    var newPosition = new Vector3(newX, newY);

        //    var player = _sceneContainer.Resolve<OldPacman>();
        //    var inputActions = _sceneContainer.Resolve<PlayerInputActions>();
        //    player.transform.position = newPosition;
        //    //player.transform.rotation = Quaternion.identity;
        //    //player.gameObject.SetActive(true);
        //    var timeService = _sceneContainer.Resolve<TimeService>();
        //    //player.Initialize(_mapHandler, inputActions, timeService);

        //    // Не изменять. Понадобится для спавна игрока при смерти
        //    //_mapHandler.ChangeTile(new Vector3(x, y + 1), GameConstants.EmptyTile);
        //}

        //private void SpawnPacmanTest(float x, float y)
        //{
        //    var Pacman = _gameState.Map.Value.Entities.First(entity => entity.Type == EntityType.Pacman);
        //    var gameStateService = _sceneContainer.Resolve<IGameStateService>();

        //    var player = _sceneContainer.Resolve<OldPacmanView>();
        //    var inputActions = _sceneContainer.Resolve<PlayerInputActions>();
        //    var timeService = _sceneContainer.Resolve<TimeService>();

        //    float newX = 0;
        //    float newY = 0;

        //    if (Pacman.Position.Value == Vector2.zero)     // Позицию определять на этапе загрузки?
        //    {
        //        newX = x * GameConstants.GridCellSize + GameConstants.GridCellSize * GameConstants.Half;
        //        newY = -(y * GameConstants.GridCellSize - GameConstants.GridCellSize * GameConstants.Half);
        //    }
        //    else
        //    {
        //        var position = Pacman.Position.Value;
        //        newX = position.x * GameConstants.GridCellSize + GameConstants.GridCellSize * GameConstants.Half;
        //        newY = -(position.y * GameConstants.GridCellSize - GameConstants.GridCellSize * GameConstants.Half);
        //    }

        //    player.transform.rotation = Quaternion.identity;
        //    player.transform.position = new Vector3(newX, newY);

        //    //player.Bind(Pacman as Pacman, inputActions, gameStateService, _mapHandler, timeService);

        //    player.gameObject.SetActive(true);
        //}
    }
}
