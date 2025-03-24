using ObservableCollections;
using R3;
using System;
using System.Linq;
using UnityEngine;
using Game.State.Root;
using Game.State.Buildings;

namespace Game.Services
{
    // Сервис уровня сцены
    public class SomeGameplayService : IDisposable
    {
        private readonly GameStateProxy _gameState;
        private readonly SomeCommonService _someCommonService;

        // Ему для создания необходим некий другой сервис, в данном случае сервис уровня проекта
        public SomeGameplayService(GameStateProxy gameState, SomeCommonService someCommonService)
        {
            _gameState = gameState;
            _someCommonService = someCommonService;

            Debug.Log(GetType().Name + " has been created.");       // Заглушка ++++++++++++++++++++++++++++++++++++

            // Пробегаемся по всем строениям что уже загруженны и выводим их TypeId
            gameState.Buildings.ForEach(building => Debug.Log($"Building:{building.TypeId}"));       // Заглушка ++++++++++++++++++++++++++++++++++++
            // Подписываемся на добавления строений
            gameState.Buildings.ObserveAdd().Subscribe(element => Debug.Log($"Building added: {element.Value.TypeId}"));      // Заглушка ++++++++++++++++++++++++++++++++++++
            // Подписываемся на удаление строений
            gameState.Buildings.ObserveRemove().Subscribe(element => Debug.Log($"Building removed: {element.Value.TypeId}"));      // Заглушка ++++++++++++++++++++++++++++++++++++

            AddBuilding("test1");
            AddBuilding("test2");
            AddBuilding("test3");

            RemoveBuilding("test2");
        }

        // Вызывается контейнером перед его удалением, для этого необходимо регистрировать как single
        public void Dispose()
        {
            Debug.Log("Подчистить все подписки.");       // Заглушка ++++++++++++++++++++++++++++++++++++       
        }

        //for tests  создание  нового строения
        private void AddBuilding(string buildingTypeId)
        {
            var building = new BuildingEntity()
            {
                TypeId = buildingTypeId,
            };

            var buildingProxy = new BuildingEntityProxy(building);
            _gameState.Buildings.Add(buildingProxy);
        }

        // for tests удаление имеющегося строения
        private void RemoveBuilding(string buildingTypeId)
        {
            var buildingEntity = _gameState.Buildings.FirstOrDefault(b => b.TypeId == buildingTypeId);

            if (buildingEntity != null)
            {
                _gameState.Buildings.Remove(buildingEntity);
            }
        }
    }
}