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
                    return new SmallPelletViewModel(entity as SmallPellet);

                default:
                    throw new System.Exception($"Unsuported entity type: {entity.Type}");       // Magic
            }
        }
    }
}
