using UnityEngine;

namespace Game.Services
{
    // Сервиса уровня проекта
    public class SomeCommonService
    {
        // Например провайдер состояния, или провайдер настроек, сервис аналитики
        public SomeCommonService()
        {
            Debug.Log(GetType().Name + " has been created.");       // Заглушка
        }
    }
}
