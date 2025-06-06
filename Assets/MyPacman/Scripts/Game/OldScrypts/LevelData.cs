using UnityEngine;

namespace MyPacman
{
    public class LevelData
    {
        private int[,] _map;

        // Сделать реактивными свойствами?
        public int Score { get; private set; }
        public int Hearts { get; private set; }

        public Ghost[] Ghosts;
        public Pacman Pacman;           // Отделить игровую модель от методов

        public LevelData(ILevelConfig config)
        {
            _map = new int[config.Map.GetLength(0), config.Map.GetLength(1)];

            for (int y = 0; y < config.Map.GetLength(0); y++)
                for (int x = 0; x < config.Map.GetLength(1); x++)
                    _map[y, x] = config.Map[y, x];

            // Создать\получить pacman и призараков
        }

        public int GetTile(int x, int y)
        {
            return _map[y, x];
        }

        public void ChangeTile(Vector2Int position, int tileType)
        {
            _map[position.y, position.x] = tileType;
        }
    }
}