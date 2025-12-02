using R3;
using System;

namespace MyPacman
{
    public class Fruit : Edible
    {
        public readonly ReactiveProperty<float> TimeExists;     // Сколько времени существует данный объект
        public readonly ReactiveProperty<bool> IsFlashing;      // Мигание объекта

        private readonly FruitData _data;

        public Fruit(FruitData data) : base(data)
        {
            _data = data;
            TimeExists = new ReactiveProperty<float>(_data.TimeExists);
            IsFlashing = new ReactiveProperty<bool>(false);
            TimeExists.Subscribe(HandleState);
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

        private void HandleState(float timeExists)
        {
            _data.TimeExists = timeExists;
            float percent = TimeExists.Value / GameConstants.FruitLifespan * 100f;

            if (percent < GameConstants.PercentageForFruitFlashing)
                IsFlashing.OnNext(true);                            // Включить мигание
        }
    }
}
