using UnityEngine;

namespace Game.Services
{
    public class SomeMainMenuService
    {
        private readonly SomeCommonService _someCommonService;

        // Ему для создания необходим некий другой сервис, в данном случае сервис уровня проекта
        public SomeMainMenuService(SomeCommonService someCommonService)
        {
            _someCommonService = someCommonService;
            Debug.Log(GetType().Name + " has been created.");       // Заглушка
        }
    }
}
