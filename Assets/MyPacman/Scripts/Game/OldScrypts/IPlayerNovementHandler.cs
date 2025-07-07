using System;
using UnityEngine;

namespace MyPacman
{
    public interface IPlayerMovementHandler
    {
        public event Action<Vector3Int> TileChanged;
        public void Tick();             // Сделать общий Update через R3?
        public void Movement();
        public void StartMoving();
        public void StopMoving();
        //public void Initialyze(Func<Vector2> getDirection, Vector2 mapSize);
    }
}