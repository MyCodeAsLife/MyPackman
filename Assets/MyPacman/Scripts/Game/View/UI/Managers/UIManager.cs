
namespace MyPacman
{
    // Тот кто знает как создавать вьюмодели для окон
    public abstract class UIManager
    {
        // Чтобы вытаскивать все необходимое для сбора вьюмоделей окошек
        protected readonly DIContainer SceneContainer;

        protected UIManager(DIContainer container)
        {
            SceneContainer = container;
        }
    }
}
