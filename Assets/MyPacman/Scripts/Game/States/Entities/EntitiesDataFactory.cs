using System;
using UnityEngine;

namespace MyPacman
{
    public class EntitiesDataFactory
    {
        public EntityData CreateEntityData(Func<int> createEntityId, Vector2 position, EntityType entityType)
        {
            var entityData = CreateEntityData(entityType);
            entityData = InitEntityData(entityData, entityType, createEntityId, position);          // entityType - лишний, в data уже записан entityType
            return entityData;
        }

        private EntityData CreateEntityData(EntityType type)
        {
            EntityData entityData = null;

            if (type == EntityType.Pacman)
                entityData = new PacmanData();
            else if (type <= EntityType.SmallPellet && type >= EntityType.LargePellet)
                entityData = new PelletData();
            else if (type <= EntityType.Blinky && type >= EntityType.Clyde)
                entityData = new GhostData();
            else if (type <= EntityType.Cherry && type >= EntityType.Key)
                entityData = new FruitData();
            else
                throw new System.Exception($"Unsuported entity type: {type}");       // Magic

            entityData.Type = type;
            return entityData;
        }

        private EntityData InitEntityData(
            EntityData entityData,
            EntityType entityType,
            Func<int> createEntitytId,
            Vector2 position)
        {
            string path = "";
            entityData.PositionX = position.x;
            entityData.PositionY = position.y;
            entityData.UniqId = createEntitytId();

            if (entityType == EntityType.Pacman)
                path = GameConstants.PacmanFolderPath;
            else if (entityType <= EntityType.SmallPellet && entityType >= EntityType.LargePellet)
                path = GameConstants.PelletsFolderPath;
            else if (entityType <= EntityType.Blinky && entityType >= EntityType.Clyde)
                path = GameConstants.GhostsFolderPath;
            else if (entityType <= EntityType.Cherry && entityType >= EntityType.Key)
                path = GameConstants.FruitsFolderPath;
            else
                throw new Exception($"Unsuported entity type: {entityType}");       // Magic


            if (entityData is EdibleData)
            {
                var edibleData = entityData as EdibleData;
                edibleData.Points = DefineEdibleEntityPoints(edibleData.Type);
            }

            entityData.PrefabPath = path + entityData.Type.ToString();
            return entityData;
        }

        private EdibleEntityPoints DefineEdibleEntityPoints(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.SmallPellet:
                    return EdibleEntityPoints.SmallPellet;                      // Переделать, у каждой пеллеты свои очки

                case EntityType.MediumPellet:
                case EntityType.LargePellet:
                    return EdibleEntityPoints.MediumPellet;                      // Переделать, у каждой пеллеты свои очки

                case EntityType.Blinky:
                case EntityType.Pinky:
                case EntityType.Inky:
                case EntityType.Clyde:
                    return EdibleEntityPoints.Ghost;

                case EntityType.Cherry:
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