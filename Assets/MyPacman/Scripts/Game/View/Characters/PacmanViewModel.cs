using System;
using UnityEngine;

namespace MyPacman
{
    public class PacmanViewModel : EntityViewModel
    {
        public PacmanViewModel(PacmanEntity pacmanEntity) : base(pacmanEntity) { }

        // Передать функцию запроса позиции
        public void PassPositionRequestFunction(Func<Vector2> getCurrentPosition)
        {
            (Entity as PacmanEntity).PassPositionRequestFunction(getCurrentPosition);
        }
    }
}
