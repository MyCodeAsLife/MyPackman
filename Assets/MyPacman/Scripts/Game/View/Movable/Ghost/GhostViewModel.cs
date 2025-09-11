using R3;
using System;

namespace MyPacman
{
    public class GhostViewModel : MovableEntityViewModel
    {
        public readonly ReadOnlyReactiveProperty<GhostBehaviorModeType> CurrentBehaviorMode;    // Переименовать

        public GhostViewModel(Entity entity) : base(entity)
        {
            var ghost = entity as Ghost;
            CurrentBehaviorMode = ghost.CurrentBehaviorMode;
        }

        //public void PassGhostBody(SpriteRenderer ghostBody)
        //{
        //    (Entity as Ghost).PassGhostBody(ghostBody);
        //}

        internal void PassFuncHideGhost(Action hideGhost)
        {
            (Entity as Ghost).PassFuncHideGhost(hideGhost);
        }

        internal void PassFuncShowGhost(Action showGhost)
        {
            (Entity as Ghost).PassFuncShowGhost(showGhost);
        }
    }
}
