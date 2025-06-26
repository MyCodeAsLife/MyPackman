using UnityEngine;

namespace MyPacman
{
    public class GameplayRegistrations
    {
        public GameplayRegistrations(DIContainer sceneContainer, GameplayEnterParams gameplayEnterParams)
        {
            //sceneContainer.RegisterFactory<PlayerInputActions>(_ => new PlayerInputActions()).AsSingle();

            sceneContainer.RegisterFactory(_ => new CharacterFactory()).AsSingle();
            sceneContainer.RegisterFactory(c => c.Resolve<CharacterFactory>().CreatePacman(Vector3.zero)).AsSingle();               // Выпилить
            sceneContainer.RegisterFactory(c => c.Resolve<CharacterFactory>().CreatePacmanTest(Vector3.zero)).AsSingle();           // Выпилить
            sceneContainer.RegisterFactory(c => c.Resolve<CharacterFactory>().CreateGhost(Vector3.zero));                           // Выпилить

            sceneContainer.RegisterInstance<ILevelConfig>(new NormalLevelConfig());

            // Перед данной регистрацией нужно чтобы состояние уровня\карты уже было создано
            var gameStateService = sceneContainer.Resolve<IGameStateService>();
            var loadingMap = gameStateService.GameState.Map;
            sceneContainer.RegisterFactory(_ => new WorldGameplayRootViewModel(loadingMap.Value.Entities)).AsSingle();
        }
    }
}
