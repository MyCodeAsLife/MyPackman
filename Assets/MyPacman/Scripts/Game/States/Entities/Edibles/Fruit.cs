using R3;
using System;
using UnityEngine;

namespace MyPacman
{
    public class Fruit : Edible
    {
        public readonly ReactiveProperty<float> TimeExists;     // Сколько времени существует данный объект
        public readonly ReactiveProperty<bool> IsFlashing;      // Мигание объекта

        private readonly FruitData _data;

        private TimeService _timeService;

        public event Action<Fruit> TimeOfLifeIsOver;

        public Fruit(FruitData data) : base(data)
        {
            _data = data;
            TimeExists = new ReactiveProperty<float>(_data.TimeExists);
            TimeExists.Subscribe(value => _data.TimeExists = value);
            IsFlashing = new ReactiveProperty<bool>(false);
        }

        ~Fruit()            // Выяснить почему во время уровня не удаляется фрукт
        {
            _timeService.TimeHasTicked -= Tick;
            Debug.Log("Destroy Fruit");         //++++++++++++++++++++++++++++++
        }

        public Action HideGhost { get; private set; }       // Как и у Ghost, перенести в Entity чтобы убрать дублирование
        public Action ShowGhost { get; private set; }       // Как и у Ghost, перенести в Entity чтобы убрать дублирование

        // Взять теже методы у Ghost, и перенести в Entity, тем самым убрав дублирование
        public void PassFuncHideGhost(Action hideGhost)
        {
            HideGhost = hideGhost;
        }
        // Взять теже методы у Ghost, и перенести в Entity, тем самым убрав дублирование
        public void PassFuncShowGhost(Action showGhost)
        {
            ShowGhost = showGhost;
        }

        public void Init(TimeService timeService)
        {
            _timeService = timeService;
            _timeService.TimeHasTicked += Tick;
        }

        private void Tick()
        {
            TimeExists.Value += _timeService.DeltaTime;
            float percent = TimeExists.Value / GameConstants.FruitLifespan * 100f;

            if (percent > GameConstants.PercentageForFruitFlashing)
                IsFlashing.OnNext(true);                                // Включить мигание

            if (GameConstants.FruitLifespan < TimeExists.Value)
            {
                TimeOfLifeIsOver?.Invoke(this);                         // Сообщить что свое время жизни вышло
                _timeService.TimeHasTicked -= Tick;
            }
        }
    }
}
