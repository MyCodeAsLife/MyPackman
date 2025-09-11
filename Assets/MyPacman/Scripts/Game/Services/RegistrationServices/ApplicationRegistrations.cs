namespace MyPacman
{
    public class ApplicationRegistrations
    {
        public ApplicationRegistrations(DIContainer projectContainer)
        {
            // Регистрация сервиса загрузки\сохранения
            projectContainer.RegisterFactory<IGameStateService>(_ => new PlayerPrefsGameStateService()).AsSingle();
            // Регистрация управления
            projectContainer.RegisterFactory<PlayerInputActions>(_ => new PlayerInputActions()).AsSingle();

            // Вынести настройку и регистрацию таймсервиса
            var timeService = UtilitiesObject.Instance.AddComponent<TimeService>();
            // Данная привязка нужна только в геймплее(пауза при нажатии escape)
            var inputActions = projectContainer.Resolve<PlayerInputActions>();
            inputActions.Keyboard.Escape.performed += _ =>
            {
                if (timeService.IsTimeRun.CurrentValue)
                    timeService.StopTime();
                else
                    timeService.RunTime();
            };
            projectContainer.RegisterInstance(timeService);
        }
    }
}
