using System;
using UnityEngine;

namespace MyPacman
{
    public class EntitiesFactory
    {
        private EntitiesDataFactory _entitiesDataFactory = new();
        private Func<int> _createEntityId;

        public EntitiesFactory(Func<int> createEntityId)
        {
            _createEntityId = createEntityId;
        }

        public Entity CreateEntity(EntityType entityType)
        {
            var entityData = _entitiesDataFactory.CreateEntityData(_createEntityId, Vector2.zero, entityType);

            return CreateEntity(entityData);
        }

        public Entity CreateEntity(Vector2 position, EntityType entityType)
        {
            var entityData = _entitiesDataFactory.CreateEntityData(_createEntityId, position, entityType);

            return CreateEntity(entityData);
        }

        public Entity CreateEntity(EntityData entityData)    // Ресурсоемкие процессы, но делаются редко
        {
            switch (entityData.Type)
            {
                case EntityType.Pacman:
                    return new Pacman(entityData as PacmanData);

                case EntityType.SmallPellet:
                case EntityType.MediumPellet:
                case EntityType.LargePellet:
                    return new Pellet(entityData as PelletData);

                case EntityType.Blinky:
                case EntityType.Pinky:
                case EntityType.Inky:
                case EntityType.Clyde:
                    return new Ghost(entityData as GhostData);

                case EntityType.Chery:
                case EntityType.Strawberry:
                case EntityType.Orange:
                case EntityType.Apple:
                case EntityType.Melon:
                case EntityType.GalaxianStarship:
                case EntityType.Bell:
                case EntityType.Key:
                    return new Fruit(entityData as FruitData);

                default:
                    throw new System.Exception($"Unsuported entity type: {entityData.Type}");       // Magic
            }
        }
    }
}