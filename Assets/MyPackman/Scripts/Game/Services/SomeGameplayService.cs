using System;
using UnityEngine;

namespace Game.Services
{
    // Сервис уровня сцены
    public class SomeGameplayService : IDisposable
    {
        private readonly SomeCommonService _someCommonService;

        // Ему для создания необходим некий другой сервис, в данном случае сервис уровня проекта
        public SomeGameplayService(SomeCommonService someCommonService)
        {
            _someCommonService = someCommonService;
            Debug.Log(GetType().Name + " has been created.");       // Заглушка
        }

        public void Dispose()
        {
            Debug.Log("Подчистить все подписки.");       // Заглушка
        }
    }
}