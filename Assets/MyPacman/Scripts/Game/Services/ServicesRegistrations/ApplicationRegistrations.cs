namespace MyPacman
{
    public class ApplicationRegistrations
    {
        public ApplicationRegistrations(DIContainer projectContainer)
        {
            var entitiesFactory = new EntitiesFactory();
            projectContainer.RegisterInstance(entitiesFactory);                  // Фабрика сущностей
            projectContainer.RegisterFactory<IGameStateService>(
                _ => new PlayerPrefsGameStateService(entitiesFactory)
                ).AsSingle();  // Регистрация сервиса загрузки\сохранения

            projectContainer.RegisterFactory<PlayerInputActions>(_ => new PlayerInputActions()).AsSingle();

            // Вынести настройку и регистрацию таймсервиса
            var timeService = UtilitiesObject.Instance.AddComponent<TimeService>();
            var inputActions = projectContainer.Resolve<PlayerInputActions>();
            inputActions.Keyboard.Escape.performed += _ =>
            {
                if (timeService.IsTimeRun)
                    timeService.StopTime();
                else
                    timeService.RunTime();
            };
            projectContainer.RegisterInstance(timeService);
        }
    }
}
