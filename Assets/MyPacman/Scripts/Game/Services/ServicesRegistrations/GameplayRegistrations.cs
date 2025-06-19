using UnityEngine;

namespace MyPacman
{
    public class GameplayRegistrations
    {
        public GameplayRegistrations(DIContainer sceneContainer, GameplayEnterParams gameplayEnterParams)
        {
            //var gameStateProvider = sceneContainer.Resolve<IGameStateService>();
            //sceneContainer.RegisterFactory<PlayerInputActions>(_ => new PlayerInputActions()).AsSingle();

            sceneContainer.RegisterFactory(_ => new CharacterFactory()).AsSingle();
            sceneContainer.RegisterFactory<Pacman>(c => c.Resolve<CharacterFactory>().CreatePacman(Vector3.zero)).AsSingle();
            sceneContainer.RegisterFactory<PacmanView>(c => c.Resolve<CharacterFactory>().CreatePacmanTest(Vector3.zero)).AsSingle();
            sceneContainer.RegisterFactory<Ghost>(c => c.Resolve<CharacterFactory>().CreateGhost(Vector3.zero));

            sceneContainer.RegisterInstance<ILevelConfig>(new NormalLevelConfig());
        }
    }
}
