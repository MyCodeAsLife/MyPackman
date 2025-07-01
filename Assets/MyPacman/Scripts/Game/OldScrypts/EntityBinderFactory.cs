using UnityEngine;

namespace MyPacman
{
    public class EntityBinderFactory       // Выпилить, устарела
    {
        public PacmanBinder CreatePacman(Vector3 position)
        {
            var pacman = Resources.Load<PacmanBinder>(GameConstants.PacmanFullPath);
            pacman = Object.Instantiate(pacman, position, Quaternion.identity);
            pacman.gameObject.SetActive(true);
            return pacman;
        }

        //public OldPacmanView CreatePacmanTest(Vector3 position)
        //{
        //    var pacman = Resources.Load<OldPacmanView>(GameConstants.PacmanTestFullPath);
        //    pacman = Object.Instantiate(pacman, position, Quaternion.identity);
        //    pacman.gameObject.SetActive(true);
        //    return pacman;
        //}

        //public Ghost CreateGhost(Vector3 position)
        //{
        //    var ghost = Resources.Load<Ghost>(GameConstants.GhostFullPath);
        //    ghost = Object.Instantiate(ghost, position, Quaternion.identity);
        //    ghost.gameObject.SetActive(true);
        //    return ghost;
        //}
    }
}