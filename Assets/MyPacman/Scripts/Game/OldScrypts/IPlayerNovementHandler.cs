using System;
using UnityEngine;

namespace MyPacman
{
    public interface IPlayerMovementHandler
    {
        public void Tick();             // Сделать общий Update через R3?
        public void Move();
        public void StartMoving();
        public void StopMoving();
        public void Initialyze(Func<Vector2> getDirection, Func<Vector2, string, bool> isObstacleTile);
    }
}