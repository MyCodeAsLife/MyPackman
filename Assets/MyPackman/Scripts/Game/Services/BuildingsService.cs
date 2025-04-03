//using Game.Gameplay.Commands;
//using Game.Gameplay.View;
//using Game.Settings.Gameplay.Buildings;
//using Game.State.Buildings;
//using Game.State.cmd;
//using ObservableCollections;
//using R3;
//using System;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Game.Services
//{
//    // Сервис это прослойка между сотоянием(реактивное, buildings и cmd) и ViewModel
//    // В данном случае команды будут передоватся\обрабатыватся через ICommandProcessor
//    public class BuildingsService
//    {
//        private readonly ICommanProcessor _cmd;
//        private readonly ObservableList<BuildingViewModel> _allBuildings = new();
//        // Кэшируем ViewModel при их создании
//        private readonly Dictionary<int, BuildingViewModel> _buildingsMap = new();
//        // Кэшируем настройки всех строений
//        private readonly Dictionary<string, BuildingSettings> _buildingsSettingsMap = new();

//        public IObservableCollection<BuildingViewModel> AllBuildings => _allBuildings;  // На вывод (ViewModel)

//        // на ввод: реактивный список состояний строений(buildings) и процессор по обработке команд(cmd)
//        public BuildingsService(IObservableCollection<BuildingEntityProxy> buildings,
//                                BuildingsSettings buildingsSettings,
//                                ICommanProcessor cmd)
//        {
//            _cmd = cmd;

//            // Кэшируем настройки всех строений
//            foreach (var buildingSettings in buildingsSettings.AllBuildings)
//            {
//                _buildingsSettingsMap[buildingSettings.TypeId] = buildingSettings;
//            }

//            // Пробегаемся по списку строений и для каждого создаем ViewModel
//            foreach (var buildingEntity in buildings)
//            {
//                CreateBuildingViewModel(buildingEntity);
//            }

//            // Подписываемся на добавление\создание нового BuildingEntityProxy
//            buildings.ObserveAdd().Subscribe(e =>
//            {
//                CreateBuildingViewModel(e.Value);   // Для новосозданного BuildingEntityProxy создаем ViewModel
//            });

//            // Подписываемся на удаление BuildingEntityProxy
//            buildings.ObserveRemove().Subscribe(e =>
//            {
//                RemoveBuildingViewModel(e.Value);   // У удаляемого объекта, удаляем ViewModel
//            });
//        }

//        public bool PlaceBuilding(string buildingTypeId, Vector3Int position)
//        {
//            var command = new CmdPlaceBuilding(buildingTypeId, position);
//            var result = _cmd.Procces(command);

//            return result;
//        }

//        public bool MoveBuilding(int buildingEntityId, Vector3Int position)
//        {
//            throw new NotImplementedException();
//        }

//        public bool DeleteBuilding(int buildingEntityId)
//        {
//            throw new NotImplementedException();
//        }

//        private void CreateBuildingViewModel(BuildingEntityProxy buildingEntity)
//        {
//            var buldingViewModel = new BuildingViewModel(buildingEntity,
//                                                        _buildingsSettingsMap[buildingEntity.TypeId],
//                                                        this);

//            _allBuildings.Add(buldingViewModel);
//            _buildingsMap[buildingEntity.Id] = buldingViewModel;
//        }

//        private void RemoveBuildingViewModel(BuildingEntityProxy buldingEntity)
//        {
//            if (_buildingsMap.TryGetValue(buldingEntity.Id, out var buldingViewModel))
//            {
//                _allBuildings.Remove(buldingViewModel);
//                _buildingsMap.Remove(buldingEntity.Id);
//            }
//        }
//    }
//}
