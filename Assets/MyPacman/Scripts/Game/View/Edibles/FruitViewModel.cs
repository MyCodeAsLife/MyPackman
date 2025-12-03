using R3;
using System;

namespace MyPacman
{
    public class FruitViewModel : EntityViewModel
    {
        public readonly ReadOnlyReactiveProperty<bool> IsFlashing;

        public FruitViewModel(Entity entity) : base(entity)
        {
            var fruit = entity as Fruit;
            IsFlashing = fruit.IsFlashing;
        }
    }
}
