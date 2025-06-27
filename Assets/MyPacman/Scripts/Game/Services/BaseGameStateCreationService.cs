using System.Numerics;

namespace MyPacman
{
    public class BaseGameStateCreationService
    {
        private ILevelConfig _levelConfig;
        private EntitiesDataFactory _entitiesDataFactory;

        public GameState Create(ILevelConfig leveleConfig, EntitiesDataFactory entitiesDataFactory)
        {
            _levelConfig = leveleConfig;
            _entitiesDataFactory = entitiesDataFactory;

            var gameStateData = new GameStateData();
            gameStateData.Score = 0;
            gameStateData.HigthScore = 0;                                       // Подгружать из сохранения
            gameStateData.LifePoints = GameConstants.StartLifePointsAmount;
            gameStateData.NumberOfCollectedFruits = 0;

            gameStateData.Map = new MapData();
            gameStateData.Map.Entities = new();
            gameStateData.Map.LevelNumber = GameConstants.StartingDifficultyLevel;
            gameStateData.Map.NumberOfFruits = 0;
            gameStateData.Map.NumberOfPellets = 0;
            gameStateData.Map.NumberOfCollectedPellets = 0;

            for (int y = 0; y < _levelConfig.Map.GetLength(0); y++)
            {
                for (int x = 0; x < _levelConfig.Map.GetLength(1); x++)
                {
                    int entityNumber = _levelConfig.Map[y, x];

                    switch (entityNumber)
                    {
                        case (int)EntityType.SmallPellet:
                        case (int)EntityType.MediumPellet:
                            gameStateData.Map.Entities.Add(CretePelletData(
                                gameStateData,
                                new Vector2(x, y),
                                (EntityType)entityNumber));
                            break;

                        case (int)EntityType.Fruit:
                            gameStateData.Map.Entities.Add(CreateFruitData(gameStateData, new Vector2(x, y)));
                            break;

                        case (int)EntityType.Ghost:
                            gameStateData.Map.Entities.Add(CreateChostData(new Vector2(x, y)));
                            break;

                        case (int)EntityType.Pacman:
                            gameStateData.Map.Entities.Add(CreatePacmanData(new Vector2(x, y)));
                            break;
                    }
                }
            }

            return new GameState(gameStateData);
        }

        private EntityData CreatePacmanData(Vector2 position)
        {
            // Проверить соседние клетки по часовой стрелке, если такиеже тайлы то выбрать позицию между ними
            var entityData = CreateEntityData(position, EntityType.Pacman);
            entityData.PrefabPath = GameConstants.PacmanNewFullPath;
            return entityData;
        }

        private EntityData CreateChostData(Vector2 position)
        {
            // Проверить соседние клетки по часовой стрелке, если такиеже тайлы то выбрать позицию между ними
            var entityData = CreateEntityData(position, EntityType.Ghost);
            entityData.PrefabPath = GameConstants.GhostFullPath;
            return entityData;
        }

        private EntityData CretePelletData(GameStateData gameStateData, Vector2 position, EntityType entityType)
        {
            var entityData = CreateEntityData(position, entityType);
            entityData.PrefabPath = GameConstants.PelletRuleTilesFolderPath + entityType.ToString();
            gameStateData.Map.NumberOfPellets++;
            return entityData;
        }

        private EntityData CreateFruitData(GameStateData gameStateData, Vector2 position)
        {
            var entityData = CreateEntityData(position, EntityType.Fruit);
            entityData.PrefabPath = GameConstants.FruitRuleTileFolderPath + EntityType.Ghost.ToString();
            gameStateData.Map.NumberOfFruits++;
            return entityData;
        }

        private EntityData CreateEntityData(Vector2 position, EntityType entityType)
        {
            var entityData = _entitiesDataFactory.CreateEntityData(EntityType.Pacman);
            entityData.PositionX = position.X;
            entityData.PositionY = position.Y;
            entityData.Type = entityType;
            entityData.PrefabPath = GameConstants.PacmanNewFullPath;
            return entityData;
        }
    }
}
