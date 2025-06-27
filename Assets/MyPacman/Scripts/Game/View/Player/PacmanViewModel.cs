using System;
using UnityEngine;

namespace MyPacman
{
    public class PacmanViewModel : EntityViewModel
    {
        public PacmanViewModel(Pacman Pacman) : base(Pacman) { }

        // Передать функцию запроса позиции
        public void PassPositionRequestFunction(Func<Vector2> getCurrentPosition)
        {
            (Entity as Pacman).PassPositionRequestFunction(getCurrentPosition);
        }
    }
}
