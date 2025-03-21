using UnityEngine;

namespace DI
{
    // for test.
    public class testDIExampleScene : MonoBehaviour
    {
        private DIContainer _sceneContainer;

        public void Init(DIContainer projectContainer)
        {
            // Используем уровень проекта(то что было зарегестрированно на уровне проекта), чтобы вытащить то, что нам нужно.
            //var serviceWithoutTag = projectContainer.Resolve<MyAwesomeProjectService>();
            //var service1 = projectContainer.Resolve<MyAwesomeProjectService>("option 1");
            //var service2 = projectContainer.Resolve<MyAwesomeProjectService>("option 2");

            // Вытаскиваем из уровня проекта через наследование, из уровня сцены, то что нам нужно.
            _sceneContainer = new DIContainer(projectContainer);
            var serviceWithoutTag = _sceneContainer.Resolve<MyAwesomeProjectService>();
            var service1 = _sceneContainer.Resolve<MyAwesomeProjectService>("option 1");
            var service2 = _sceneContainer.Resolve<MyAwesomeProjectService>("option 2");

            _sceneContainer.RegisterInstance(new MyAwesomeObject("instance", 10));                              // Регистрируем конкретный объект.

            var objectFactory = _sceneContainer.Resolve<MyAwesomeFactory>();                                    // Получаем фабрику из контейнера

            for (int i = 0; i < 5; i++)                                                                        // Затем через фабрику создаем объекты
            {
                var id = $"o{i}";
                var obj = objectFactory.CreateInstance(id, i);
                Debug.Log($"Object created with factory.\n{obj}");
            }

            var instance = _sceneContainer.Resolve<MyAwesomeObject>();                                          //Получаем объект из контейнера
            Debug.Log($"Object instance.\n{instance}");
        }
    }
}
