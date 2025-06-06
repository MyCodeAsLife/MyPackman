namespace MyPacman
{
    public class MainMenuExitParams
    {
        public MainMenuExitParams(SceneEnterParams targetSceneEnterParams)
        {
            TargetSceneEnterParams = targetSceneEnterParams;
        }

        public SceneEnterParams TargetSceneEnterParams { get; }
    }
}