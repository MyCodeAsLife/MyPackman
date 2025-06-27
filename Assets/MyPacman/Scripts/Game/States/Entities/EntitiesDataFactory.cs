namespace MyPacman
{
    public class EntitiesDataFactory
    {
        public EntityData CreateEntityData(EntityType type)
        {
            switch (type)
            {
                case EntityType.Pacman:
                    return new PacmanData();

                case EntityType.Ghost:
                    return new GhostData();

                case EntityType.SmallPellet:
                    return new SmallPelletData();

                case EntityType.MediumPellet:
                    return new MediumPelletData();

                case EntityType.LargePellet:
                    return new LargePelletData();

                case EntityType.Fruit:
                    return new FruitData();

                default:
                    throw new System.Exception($"Unsuported entity type: {type}");       // Magic
            }
        }
    }
}
