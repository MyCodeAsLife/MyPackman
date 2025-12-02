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

        public void PassFuncHideGhost(Action hideGhost)
        {
            (Entity as Fruit).PassFuncHideGhost(hideGhost);
        }

        public void PassFuncShowGhost(Action showGhost)
        {
            (Entity as Fruit).PassFuncShowGhost(showGhost);
        }
    }
}
