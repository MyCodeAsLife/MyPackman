namespace MyPacman
{
    public abstract class SceneEnterParams
    {
        protected SceneEnterParams(ILevelConfig levelConfig)
        {
            LevelConfig = levelConfig;
        }

        public ILevelConfig LevelConfig { get; }

        public T As<T>() where T : SceneEnterParams
        {
            return (T)this;
        }
    }
}