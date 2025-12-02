using R3;
using System;
using UnityEngine;

namespace MyPacman
{
    public class Ghost : Edible, IMovable
    {
        public Action HideGhost { get; private set; }   // Как и у Fruit, перенести в Entity чтобы убрать дублирование
        public Action ShowGhost { get; private set; }   // Как и у Fruit, перенести в Entity чтобы убрать дублирование

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
            BehaviorTimer = new ReactiveProperty<float>(data.BehaviorTimer);
            BehaviorTimer.Subscribe(newTime => data.BehaviorTimer = newTime);
        }

        public ReactiveProperty<bool> IsMoving { get; private set; }
        public ReactiveProperty<Vector2> Direction { get; private set; }
        public ReactiveProperty<float> SpeedModifier { get; private set; }
        //New
        public ReactiveProperty<GhostBehaviorModeType> CurrentBehaviorMode { get; private set; }        // Переименовать
        public ReactiveProperty<float> BehaviorTimer { get; private set; }
        // Взять теже методы у Fruit, и перенести в Entity, тем самым убрав дублирование
        public void PassFuncHideGhost(Action hideGhost)
        {
            HideGhost = hideGhost;
        }
        // Взять теже методы у Fruit, и перенести в Entity, тем самым убрав дублирование
        public void PassFuncShowGhost(Action showGhost)
        {
            ShowGhost = showGhost;
        }
    }
}
