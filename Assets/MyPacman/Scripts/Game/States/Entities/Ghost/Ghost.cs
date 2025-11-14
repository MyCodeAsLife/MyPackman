using R3;
using System;
using UnityEngine;

namespace MyPacman
{
    public class Ghost : Edible, IMovable
    {
        public Action HideGhost { get; private set; }
        public Action ShowGhost { get; private set; }

        public Ghost(GhostData data) : base(data)
        {
            IsMoving = new ReactiveProperty<bool>(data.IsMoving);
            IsMoving.Subscribe(isMoving => data.IsMoving = isMoving);
            Direction = new ReactiveProperty<Vector2>(data.Direction);
            Direction.Subscribe(direction => data.Direction = new Vector2Int((int)direction.x, (int)direction.y));
            SpeedModifier = new ReactiveProperty<float>(data.SpeedModifier);
            SpeedModifier.Subscribe(speedModifier => data.SpeedModifier = speedModifier);
            // New
            CurrentBehaviorMode = new ReactiveProperty<GhostBehaviorModeType>(data.CurrentBehaviorMode);
            CurrentBehaviorMode.Subscribe(newBehaviorMode => data.CurrentBehaviorMode = newBehaviorMode);
        }

        //public SpriteRenderer GhostBody { get; private set; }
        public ReactiveProperty<bool> IsMoving { get; private set; }
        public ReactiveProperty<Vector2> Direction { get; private set; }
        public ReactiveProperty<float> SpeedModifier { get; private set; }
        //New
        public ReactiveProperty<GhostBehaviorModeType> CurrentBehaviorMode { get; private set; }        // Переименовать
        //public Vector2 SpawnPosition { get; private set; }          // Нужно ли его делать реактивным?

        //public void PassGhostBody(SpriteRenderer ghostBody)             // Переименовать ?
        //{
        //    GhostBody = ghostBody;
        //}

        public void PassFuncHideGhost(Action hideGhost)
        {
            HideGhost = hideGhost;
        }

        public void PassFuncShowGhost(Action showGhost)
        {
            ShowGhost = showGhost;
        }
    }
}
