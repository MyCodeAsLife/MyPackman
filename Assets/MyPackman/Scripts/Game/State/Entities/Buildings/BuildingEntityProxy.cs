using Game.State.Entities.Buildings;
using R3;
using UnityEngine;

namespace Game.State.Buildings
{
    // Для фиксации изменений будет использоватся паттерн Proxy
    public class BuildingEntityProxy
    {
        // Данные которые не меняются в процессе
        public int Id { get; }
        public string TypeId { get; }
        public BuildingEntity Origin { get; }

        // Данные которые меняются в процессе
        public ReactiveProperty<Vector3Int> Position { get; }
        public ReactiveProperty<int> Level { get; }

        public BuildingEntityProxy(BuildingEntity buildingEntity)
        {
            Origin = buildingEntity;
            Id = buildingEntity.Id;
            TypeId = buildingEntity.TypeId;
            Position = new ReactiveProperty<Vector3Int>(buildingEntity.Position);
            Level = new ReactiveProperty<int>(buildingEntity.Level);

            // Пропускаем срабатывание ивента при подписке
            // Подписываем лямбду которая будет присваивать новое значение в указанную переменную
            Position.Skip(1).Subscribe(value => buildingEntity.Position = value);
            // Тоже самое делаем с уровнем
            Level.Skip(1).Subscribe(value => buildingEntity.Level = value);
        }
    }
}
