using System;
using UnityEngine;

namespace Game.State.Buildings
{
    [Serializable]  // Для сохранения состояния объекта, для упаковывания в json
    public class BuildingEntity
    {
        public int Id;
        public string TypeId;
        public Vector3Int Position;
        public int Level;           // Уровень здания \ грейд здания
    }
}
