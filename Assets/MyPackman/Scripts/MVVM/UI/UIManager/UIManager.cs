using DI;

namespace MVVM.UI.UIManager
{
    public abstract class UIManager
    {
        // Чтобы вытаскивать необходимое и собирать ViewModel окон
        protected readonly DIContainer Container;

        public UIManager(DIContainer container)
        {
            Container = container;
        }
    }
}
