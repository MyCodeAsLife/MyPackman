namespace MyPacman
{
    public abstract class SceneEnterParams
    {
        protected SceneEnterParams(string sceneName)
        {
            SceneName = sceneName;
        }

        public string SceneName { get; }

        public T As<T>() where T : SceneEnterParams
        {
            return (T)this;
        }
    }
}