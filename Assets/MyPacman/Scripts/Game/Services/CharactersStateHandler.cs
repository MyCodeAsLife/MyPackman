using System.Collections.Generic;

namespace MyPacman
{
    // Обрабатывает состояния персонажей
    // Переключает режимы призраков
    // Спавнит персонажа при смерти или вызывает завершение игры при недостатке очков жизни
    public class CharactersStateHandler
    {
        private readonly Dictionary<EntityType, Entity> _charactersMap;     // Тут должны быть службы управляющие персонажами

        public CharactersStateHandler(GameState gameState)
        {

        }
    }
}
