namespace MyPacman
{
    public class BaseGameStateCreationService
    {
        private ILevelConfig _levelConfig;
        //private EntitiesDataFactory _entitiesDataFactory;

        public GameState Create(ILevelConfig leveleConfig)
        {
            _levelConfig = leveleConfig;
            //_entitiesDataFactory = entitiesDataFactory;

            var gameStateData = new GameStateData();
            gameStateData.Score = 0;
            gameStateData.HigthScore = 0;                                       // Подгружать из сохранения
            gameStateData.LifePoints = GameConstants.StartLifePointsAmount;
            gameStateData.NumberOfCollectedFruits = 0;

            gameStateData.Map = new MapData();
            //gameStateData.Map.Entities = new();
            gameStateData.Map.LevelNumber = GameConstants.StartingDifficultyLevel;
            //gameStateData.Map.NumberOfCollectedFruits = 0;
            gameStateData.Map.NumberOfPellets = 0;
            gameStateData.Map.NumberOfCollectedPellets = 0;

            //for (int y = 0; y < _levelConfig.Map.GetLength(0); y++)
            //{
            //    for (int x = 0; x < _levelConfig.Map.GetLength(1); x++)
            //    {
            //        int entityNumber = _levelConfig.Map[y, x] - 1;

            //        if (entityNumber < 0)
            //        {
            //            Vector2 position = new Vector2(x, y);
            //            //EntityData entityData = CreateEntityData(gameStateData, position, (EntityType)entityNumber);
            //            EntityData entityData = entitiesDataFactory.CreateEntityData(
            //                gameStateData,
            //                position,
            //                (EntityType)entityNumber);
            //            gameStateData.Map.Entities.Add(entityData);
            //        }
            //    }
            //}

            return new GameState(gameStateData);
        }

        //private EntityData CreateEntityData(GameStateData gameStateData, Vector2 position, EntityType entityType)
        //{
        //    EntityData entityData;

        //    switch (entityType)
        //    {
        //        case EntityType.SmallPellet:
        //        case EntityType.MediumPellet:
        //            entityData = CretePelletData(gameStateData, position, (EntityType)entityType);
        //            break;

        //        case EntityType.Fruit:
        //            entityData = CreateFruitData(gameStateData, position);
        //            break;

        //        case EntityType.Ghost:
        //            entityData = CreateChostData(position);
        //            break;

        //        case EntityType.Pacman:
        //            entityData = CreatePacmanData(position);
        //            break;

        //        default:
        //            throw new Exception($"Error creating EntityData, unknown type{entityType}");        // Magic
        //    }

        //    return entityData;
        //}

        //private EntityData CreatePacmanData(Vector2 position)
        //{
        //    // Проверить соседние клетки по часовой стрелке, если такиеже тайлы то выбрать позицию между ними
        //    var entityData = CreateEntityDataBasis(position, EntityType.Pacman);
        //    entityData.PrefabPath = GameConstants.PacmanNewFullPath;
        //    return entityData;
        //}

        //private EntityData CreateChostData(Vector2 position)
        //{
        //    // Проверить соседние клетки по часовой стрелке, если такиеже тайлы то выбрать позицию между ними
        //    var entityData = CreateEntityDataBasis(position, EntityType.Ghost);
        //    entityData.PrefabPath = GameConstants.GhostFullPath;
        //    return entityData;
        //}

        //private EntityData CretePelletData(GameStateData gameStateData, Vector2 position, EntityType entityType)
        //{
        //    var entityData = CreateEntityDataBasis(position, entityType);
        //    entityData.PrefabPath = GameConstants.PelletRuleTilesFolderPath + entityType.ToString();
        //    gameStateData.Map.NumberOfPellets++;
        //    return entityData;
        //}

        //private EntityData CreateFruitData(GameStateData gameStateData, Vector2 position)
        //{
        //    var entityData = CreateEntityDataBasis(position, EntityType.Fruit);
        //    entityData.PrefabPath = GameConstants.FruitRuleTileFolderPath + EntityType.Ghost.ToString();
        //    gameStateData.Map.NumberOfFruits++;
        //    return entityData;
        //}

        //private EntityData CreateEntityDataBasis(Vector2 position, EntityType entityType)
        //{
        //    var entityData = _entitiesDataFactory.CreateEntityData(EntityType.Pacman);
        //    entityData.PositionX = position.X;
        //    entityData.PositionY = position.Y;
        //    entityData.Type = entityType;
        //    entityData.PrefabPath = GameConstants.PacmanNewFullPath;
        //    return entityData;
        //}
    }
}
