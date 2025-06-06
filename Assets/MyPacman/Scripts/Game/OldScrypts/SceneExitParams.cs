namespace MyPacman
{
    public class SceneExitParams
    {
        public SceneExitParams(MainMenuEnterParams mainMenuEnterParams)
        {
            MainMenuEnterParams = mainMenuEnterParams;
        }

        public MainMenuEnterParams MainMenuEnterParams { get; }
    }
}