using UnityEngine;

namespace DI
{
    // for test. Project level service
    public class MyAwesomeProjectService
    {

    }

    // for test. Scene level service
    public class MySceneService
    {
        private readonly MyAwesomeProjectService _myAwesomeProjectService;

        public MySceneService(MyAwesomeProjectService myAwesomeProjectService)
        {
            _myAwesomeProjectService = myAwesomeProjectService;
        }

    }

    // for test. Factory
    public class MyAwesomeFactory
    {
        public MyAwesomeObject CreateInstance(string id, int par1)
        {
            return new MyAwesomeObject(id, par1);
        }
    }

    // for test. Some object
    public class MyAwesomeObject
    {
        private readonly string _id;
        private readonly int _par1;

        public MyAwesomeObject(string id, int par1)
        {
            _id = id;
            _par1 = par1;
        }

        public override string ToString()
        {
            return $"Object with id:{_id} and par1:{_par1}.";
        }
    }

    // for test.
    public class testDIExamplProject : MonoBehaviour
    {
        private void Awake()
        {
            // Как бы при загрузке проекта
            var projectContainer = new DIContainer();                                               // Создаем DI-контайнер
            projectContainer.RegisterSingleton(_ => new MyAwesomeProjectService());                 // Регестрируем синглтон уровня проекта.
            projectContainer.RegisterSingleton("option 1", _ => new MyAwesomeProjectService());     // Регестрируем синглтон уровня проекта с тегом.
            projectContainer.RegisterSingleton("option 2", _ => new MyAwesomeProjectService());     // Регестрируем синглтон уровня проекта с тегом.

            // Как бы при загрузке сцены
            var sceneRoot = FindFirstObjectByType<testDIExampleScene>();
            sceneRoot.Init(projectContainer);       // Передаем в сцену контейнер проекта
        }
    }
}
