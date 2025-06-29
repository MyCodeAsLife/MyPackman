using System;
using UnityEngine;

namespace MyPacman
{
    public class EntitiesDataFactory
    {
        private Vector2 _position;
        private EntityData _entityData;

        public EntityData CreateEntityData(EntityType type)
        {
            EntityData entityData = null;

            switch (type)
            {
                case EntityType.Pacman:
                    entityData = new PacmanData();
                    break;

                case EntityType.Ghost:
                    entityData = new GhostData();
                    break;

                case EntityType.SmallPellet:
                    entityData = new SmallPelletData();
                    break;

                case EntityType.MediumPellet:
                    entityData = new MediumPelletData();
                    break;

                case EntityType.LargePellet:
                    entityData = new LargePelletData();
                    break;

                case EntityType.Fruit:
                    entityData = new FruitData();
                    break;

                default:
                    throw new System.Exception($"Unsuported entity type: {type}");       // Magic
            }

            entityData.Type = type;
            return entityData;
        }

        public EntityData CreateEntityData(GameStateData gameStateData, Vector2 position, EntityType entityType)
        {
            _entityData = CreateEntityData(entityType);
            InitEntityData();
            _position = position;

            switch (entityType)
            {
                case EntityType.Pacman:
                    _entityData = InitPacmanData();
                    break;

                case EntityType.Ghost:
                    _entityData = InitChostData();
                    break;

                case EntityType.SmallPellet:
                case EntityType.MediumPellet:
                case EntityType.LargePellet:
                    _entityData = InitPelletData(gameStateData);
                    break;

                case EntityType.Fruit:
                    _entityData = InitFruitData(gameStateData);
                    break;

                default:
                    throw new Exception($"Unsuported entity type{entityType}");        // Magic
            }

            return _entityData;
        }

        private EntityData InitPacmanData()
        {
            // Проверить соседние клетки по часовой стрелке, если такиеже тайлы то выбрать позицию между ними
            _entityData.PrefabPath = GameConstants.PacmanNewFullPath;
            return _entityData;
        }

        private EntityData InitChostData()
        {
            // Проверить соседние клетки по часовой стрелке, если такиеже тайлы то выбрать позицию между ними
            _entityData.PrefabPath = GameConstants.GhostFullPath;
            return _entityData;
        }

        private EntityData InitPelletData(GameStateData gameStateData)
        {
            _entityData.PrefabPath = GameConstants.PelletRuleTilesFolderPath + _entityData.Type.ToString();
            gameStateData.Map.NumberOfPellets++;
            return _entityData;
        }

        private EntityData InitFruitData(GameStateData gameStateData)
        {
            _entityData.PrefabPath = GameConstants.FruitRuleTilesFolderPath + EntityType.Ghost.ToString();
            gameStateData.Map.NumberOfFruits++;
            return _entityData;
        }

        private EntityData InitEntityData()
        {
            _entityData.PositionX = _position.x;
            _entityData.PositionY = _position.y;
            _entityData.PrefabPath = GameConstants.PacmanNewFullPath;
            return _entityData;
        }
    }
}
