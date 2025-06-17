namespace MyPacman
{
    public class EntitiesFactory
    {
        public Entity CreateEntity(EntityData entityData)    // Ресурсоемкие процессы, но делаются редко
        {
            switch (entityData.Type)
            {
                case EntityType.Pacman:
                    return new PacmanEntity(entityData as PacmanEntityData);

                case EntityType.Ghost:
                    return new GhostEntity(entityData as GhostEntityData);

                case EntityType.SmallPellet:
                    return new SmallPelletEntity(entityData as SmallPelletEntityData);

                case EntityType.MediumPellet:
                    return new MediumPelletEntity(entityData as MediumPelletEntityData);

                case EntityType.LargePellet:
                    return new LargePelletEntity(entityData as LargePelletEntityData);

                case EntityType.Chery:
                    return new CheryEntity(entityData as CheryEntityData);

                case EntityType.Klubnika:
                    return new KlubnikaEntity(entityData as KlubnikaEntityData);

                case EntityType.Apelsin:
                    return new ApelsinEntity(entityData as ApelsinEntityData);

                case EntityType.Apple:
                    return new AppleEntity(entityData as AppleEntityData);

                case EntityType.Avokado:
                    return new AvokadoEntity(entityData as AvokadoEntityData);

                case EntityType.KakatoHren:
                    return new KakatoHrenEntity(entityData as KakatoHrenEntityData);

                case EntityType.Kolokolchik:
                    return new KolokolchikEntity(entityData as KolokolchikEntityData);

                case EntityType.Key:
                    return new KeyEntity(entityData as KeyEntityData);

                default:
                    throw new System.Exception($"Unsuported entity type: {entityData.Type}");       // Magic
            }
        }
    }
}