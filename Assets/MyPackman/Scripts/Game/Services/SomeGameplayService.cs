using ObservableCollections;
using R3;
using System;
using System.Linq;
using UnityEngine;
using Game.State.Root;
using Game.State.Buildings;

namespace Game.Services
{
    // ������ ������ �����
    public class SomeGameplayService : IDisposable
    {
        private readonly GameStateProxy _gameState;
        private readonly SomeCommonService _someCommonService;

        // ��� ��� �������� ��������� ����� ������ ������, � ������ ������ ������ ������ �������
        public SomeGameplayService(GameStateProxy gameState, SomeCommonService someCommonService)
        {
            _gameState = gameState;
            _someCommonService = someCommonService;

            Debug.Log(GetType().Name + " has been created.");       // �������� ++++++++++++++++++++++++++++++++++++

            // ����������� �� ���� ��������� ��� ��� ���������� � ������� �� TypeId
            gameState.Buildings.ForEach(building => Debug.Log($"Building:{building.TypeId}"));       // �������� ++++++++++++++++++++++++++++++++++++
            // ������������� �� ���������� ��������
            gameState.Buildings.ObserveAdd().Subscribe(element => Debug.Log($"Building added: {element.Value.TypeId}"));      // �������� ++++++++++++++++++++++++++++++++++++
            // ������������� �� �������� ��������
            gameState.Buildings.ObserveRemove().Subscribe(element => Debug.Log($"Building removed: {element.Value.TypeId}"));      // �������� ++++++++++++++++++++++++++++++++++++

            AddBuilding("test1");
            AddBuilding("test2");
            AddBuilding("test3");

            RemoveBuilding("test2");
        }

        // ���������� ����������� ����� ��� ���������, ��� ����� ���������� �������������� ��� single
        public void Dispose()
        {
            Debug.Log("���������� ��� ��������.");       // �������� ++++++++++++++++++++++++++++++++++++       
        }

        //for tests  ��������  ������ ��������
        private void AddBuilding(string buildingTypeId)
        {
            var building = new BuildingEntity()
            {
                TypeId = buildingTypeId,
            };

            var buildingProxy = new BuildingEntityProxy(building);
            _gameState.Buildings.Add(buildingProxy);
        }

        // for tests �������� ���������� ��������
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