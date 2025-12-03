using R3;
using System;

namespace MyPacman
{
    public class Fruit : Edible
    {
        public readonly ReactiveProperty<float> TimeExists;
        public readonly ReactiveProperty<bool> IsFlashing;

        private readonly FruitData _data;

        private TimeService _timeService;

        public event Action<Fruit> TimeOfLifeIsOver;        // Переименовать под ивент
        public event Action Timer;                          // Переименовать под ивент

        public Fruit(FruitData data) : base(data)
        {
            _data = data;
            TimeExists = new ReactiveProperty<float>(_data.TimeExists);
            TimeExists.Subscribe(value => _data.TimeExists = value);
            IsFlashing = new ReactiveProperty<bool>(false);
        }

        ~Fruit()            // Выяснить почему во время уровня(после съедания) не удаляется фрукт
        {
            _timeService.TimeHasTicked -= Tick;
        }

        public void Init(TimeService timeService)
        {
            _timeService = timeService;
            _timeService.TimeHasTicked += Tick;
            Timer += CheckTimeBeforeFlashing;
            Timer += CheckTimeBeforeEndOfLifetime;
        }

        private void Tick()
        {
            Timer?.Invoke();
        }

        private void CheckTimeBeforeEndOfLifetime()
        {
            TimeExists.Value += _timeService.DeltaTime;

            if (GameConstants.FruitLifespan < TimeExists.Value)
            {
                _timeService.TimeHasTicked -= Tick;
                TimeOfLifeIsOver?.Invoke(this);
            }
        }

        private void CheckTimeBeforeFlashing()
        {
            float percent = TimeExists.Value / GameConstants.FruitLifespan * 100f;

            if (percent > GameConstants.PercentageForFruitFlashing)
            {
                Timer -= CheckTimeBeforeFlashing;
                IsFlashing.OnNext(true);
            }
        }
    }
}
