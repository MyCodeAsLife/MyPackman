namespace MyPacman
{
    public class ViewModelFactory
    {
        public EntityViewModel CreateEntityViewModel(Entity entity)    // Ресурсоемкие процессы, но делаются редко
        {
            switch (entity.Type)
            {
                case EntityType.Pacman:
                    return new PacmanViewModel(entity as Pacman);

                case EntityType.SmallPellet:
                case EntityType.MediumPellet:
                case EntityType.LargePellet:
                    return new PelletViewModel(entity as Pellet);

                case EntityType.Blinky:
                case EntityType.Pinky:
                case EntityType.Inky:
                case EntityType.Clyde:
                    return new GhostViewModel(entity as Ghost);

                case EntityType.Cherry:
                case EntityType.Strawberry:
                case EntityType.Orange:
                case EntityType.Apple:
                case EntityType.Melon:
                case EntityType.GalaxianStarship:
                case EntityType.Bell:
                case EntityType.Key:
                    return new FruitViewModel(entity as Fruit);

                default:
                    throw new System.Exception($"Unsuported entity type: {entity.Type}");       // Magic
            }
        }
    }
}
