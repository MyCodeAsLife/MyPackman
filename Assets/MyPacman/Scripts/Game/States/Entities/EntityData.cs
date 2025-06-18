using System;
using UnityEngine;

namespace MyPacman
{
    [Serializable]
    public class EntityData
    {
        public int UniqId { get; set; }             // Уникальный идентификатор сущности
        //public string ConfigId { get; set; }        // Идентификатор для поиска настроек сущности, ID конфига объека настроек ScriptableObject
        public EntityType Type { get; set; }        // Тип сущности для быстрого понимания, что это за сущность
        public Vector3Int Position { get; set; }    // Позития в координатах x,y, которая конвертируется в x,z, на плоскости
    }
}