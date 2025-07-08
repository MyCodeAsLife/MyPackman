using System.Collections.Generic;

namespace MyPacman
{
    // Обрабатывает состояния персонажей
    // Переключает режимы призраков
    // Спавнит персонажа при смерти или вызывает завершение игры при недостатке очков жизни
    public class CharactersStateHandler
    {
        private readonly Dictionary<EntityType, GhostMovementService> _ghostsMap = new();           // Тут должны быть службы управляющие персонажами
        //private readonly Dictionary<EntityType, GhostMovementService> _ghostBehaviorModeMap = new();

        public CharactersStateHandler(GameState gameState)
        {

        }

        public GhostBehaviorModeType GhostBehaviorModeType { get; private set; }
    }
}
