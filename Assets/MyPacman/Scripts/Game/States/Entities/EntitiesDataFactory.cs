using R3;
using System;
using UnityEngine;

namespace MyPacman
{
    public class EntitiesDataFactory
    {
        public EntityData CreateEntityData(GameState gameState, Vector2 position, EntityType entityType)
        {
            var entityData = CreateEntityData(entityType);
            entityData = InitEntityData(entityData, entityType, gameState, position);
            return entityData;
        }

        private EntityData CreateEntityData(EntityType type)
        {
            EntityData entityData = null;

            switch (type)
            {
                case EntityType.Pacman:
                    entityData = new PacmanData();
                    break;

                case EntityType.SmallPellet:
                case EntityType.MediumPellet:
                case EntityType.LargePellet:
                    entityData = new PelletData();
                    break;

                case EntityType.Blinky:
                case EntityType.Pinky:
                case EntityType.Inky:
                case EntityType.Clyde:
                    entityData = new GhostData();
                    break;

                case EntityType.Chery:
                case EntityType.Strawberry:
                case EntityType.Orange:
                case EntityType.Apple:
                case EntityType.Melon:
                case EntityType.GalaxianStarship:
                case EntityType.Bell:
                case EntityType.Key:
                    entityData = new FruitData();
                    break;

                default:
                    throw new System.Exception($"Unsuported entity type: {type}");       // Magic
            }

            entityData.Type = type;
            return entityData;
        }

        private EntityData InitEntityData(
            EntityData entityData,
            EntityType entityType,
            GameState gameState,
            Vector2 position)
        {
            entityData.PositionX = position.x;
            entityData.PositionY = position.y;
            entityData.UniqId = gameState.CreateEntityId();

            switch (entityType)
            {
                case EntityType.Pacman:
                    entityData = InitPacmanData(entityData);
                    break;

                case EntityType.SmallPellet:
                case EntityType.MediumPellet:
                case EntityType.LargePellet:
                    entityData = InitPelletData(entityData, gameState.Map.Value.NumberOfPellets);
                    break;

                case EntityType.Blinky:
                case EntityType.Pinky:
                case EntityType.Inky:
                case EntityType.Clyde:
                    entityData = InitChostData(entityData);
                    break;

                case EntityType.Chery:
                case EntityType.Strawberry:
                case EntityType.Orange:
                case EntityType.Apple:
                case EntityType.Melon:
                case EntityType.GalaxianStarship:
                case EntityType.Bell:
                case EntityType.Key:
                    entityData = InitFruitData(entityData, gameState.Map.Value.NumberOfFruits);
                    break;

                default:
                    throw new Exception($"Unsuported entity type{entityType}");        // Magic
            }

            return entityData;
        }

        private EntityData InitPacmanData(EntityData entityData)
        {
            // Проверить соседние клетки по часовой стрелке, если такиеже тайлы то выбрать позицию между ними
            entityData.PrefabPath = GameConstants.PacmanFullPath;
            return entityData;
        }

        private EntityData InitChostData(EntityData entityData)
        {
            var edibleData = entityData as EdibleData;
            edibleData.PrefabPath = GameConstants.GhostsFolderPath + edibleData.Type.ToString();
            edibleData.Points = DefineEdibleEntityPoints(edibleData.Type);
            return edibleData;
        }

        private EntityData InitPelletData(EntityData entityData, ReactiveProperty<int> numberOfPellets)
        {
            var edibleData = entityData as EdibleData;
            edibleData.PrefabPath = GameConstants.PelletsFolderPath + edibleData.Type.ToString();
            edibleData.Points = DefineEdibleEntityPoints(edibleData.Type);
            numberOfPellets.Value++;
            return edibleData;
        }

        private EntityData InitFruitData(EntityData entityData, ReactiveProperty<int> numberOfFruits)
        {
            var edibleData = entityData as EdibleData;
            edibleData.PrefabPath = GameConstants.FruitsFolderPath + edibleData.Type.ToString();
            edibleData.Points = DefineEdibleEntityPoints(edibleData.Type);
            numberOfFruits.Value++;
            return edibleData;
        }

        private EdibleEntityPoints DefineEdibleEntityPoints(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Chery:
                    return EdibleEntityPoints.Cherry;

                case EntityType.Strawberry:
                    return EdibleEntityPoints.Strawberry;

                case EntityType.Orange:
                    return EdibleEntityPoints.Orange;

                case EntityType.Apple:
                    return EdibleEntityPoints.Apple;

                case EntityType.Melon:
                    return EdibleEntityPoints.Melon;

                case EntityType.GalaxianStarship:
                    return EdibleEntityPoints.GalaxianStarship;

                case EntityType.Bell:
                    return EdibleEntityPoints.Bell;

                case EntityType.Key:
                    return EdibleEntityPoints.Key;

                default:
                    throw new Exception($"Unsuported entity type{entityType}");                 // Magic
            }
        }
    }
}
