using System;

namespace MyPacman
{
    [Serializable]
    public class EntityData
    {
        public int UniqId { get; set; }             // Уникальный идентификатор сущности
        //public string ConfigId { get; set; }        // Идентификатор для поиска настроек сущности, ID конфига объека настроек ScriptableObject
        public EntityType Type { get; set; }        // Тип сущности для быстрого понимания, что это за сущность

        //new
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public string PrefabPath { get; set; }
    }
}