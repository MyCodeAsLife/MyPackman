using R3;
using System;
using UnityEngine;

namespace MyPacman
{
    public class PacmanViewModel : MovableEntityViewModel
    {
        public readonly Observable<Unit> Dead;

        public PacmanViewModel(Pacman Pacman) : base(Pacman)
        {
            Dead = Pacman.Dead;
        }

        // Передать функцию запроса позиции
        public void PassPositionRequestFunction(Func<Vector2> getCurrentPosition)
        {
            (Entity as Pacman).PassPositionRequestFunction(getCurrentPosition);
        }

        public void SubscribeToDeadAnimationFinish(Observable<Unit> request)
        {
            (Entity as Pacman).SubscribeToDeadAnimationFinish(request);
        }
    }
}
