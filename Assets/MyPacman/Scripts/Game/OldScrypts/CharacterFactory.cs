﻿using UnityEngine;

namespace MyPacman
{
    public class CharacterFactory
    {
        public Pacman CreatePacman(Vector3 position)
        {
            var pacman = Resources.Load<Pacman>(GameConstants.PacmanFullPath);
            pacman = Object.Instantiate(pacman, position, Quaternion.identity);
            pacman.gameObject.SetActive(true);
            return pacman;
        }

        public Ghost CreateGhost(Vector3 position)
        {
            var ghost = Resources.Load<Ghost>(GameConstants.GhostFullPath);
            ghost = Object.Instantiate(ghost, position, Quaternion.identity);
            ghost.gameObject.SetActive(true);
            return ghost;
        }
    }
}