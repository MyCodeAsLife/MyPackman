namespace MyPacman
{
    public class ViewModelFactory
    {
        public EntityViewModel CreateEntityViewModel(Entity entity)    // Ресурсоемкие процессы, но делаются редко
        {
            switch (entity.Type)
            {
                case EntityType.Pacman:
                    return new PacmanViewModel(entity as PacmanEntity);

                //case EntityType.Ghost:
                //    return new GhostEntity(entity as GhostEntityData);

                //case EntityType.SmallPellet:
                //    return new SmallPelletEntity(entity as SmallPelletEntityData);

                //case EntityType.MediumPellet:
                //    return new MediumPelletEntity(entity as MediumPelletEntityData);

                //case EntityType.LargePellet:
                //    return new LargePelletEntity(entity as LargePelletEntityData);

                //case EntityType.Chery:
                //    return new CheryEntity(entity as CheryEntityData);

                //case EntityType.Klubnika:
                //    return new KlubnikaEntity(entity as KlubnikaEntityData);

                //case EntityType.Apelsin:
                //    return new ApelsinEntity(entity as ApelsinEntityData);

                //case EntityType.Apple:
                //    return new AppleEntity(entity as AppleEntityData);

                //case EntityType.Avokado:
                //    return new AvokadoEntity(entity as AvokadoEntityData);

                //case EntityType.KakatoHren:
                //    return new KakatoHrenEntity(entity as KakatoHrenEntityData);

                //case EntityType.Kolokolchik:
                //    return new KolokolchikEntity(entity as KolokolchikEntityData);

                //case EntityType.Key:
                //    return new KeyEntity(entity as KeyEntityData);

                default:
                    throw new System.Exception($"Unsuported entity type: {entity.Type}");       // Magic
            }
        }
    }
}
