using R3;
using System;
using System.Collections;
using UnityEngine;

namespace MyPacman
{
    public class TimeService : MonoBehaviour
    {
        // New
        private ReactiveProperty<bool> _isTimeRun = new ReactiveProperty<bool>(true);

        private float _time;
        private Coroutine _timer = null;

        public event Action TimeHasTicked;

        public ReadOnlyReactiveProperty<bool> IsTimeRun => _isTimeRun;
        //public bool IsTimeRun { get; private set; } = true;
        public float DeltaTime { get; private set; }        // Зачем это? к томуж привязанное к фиксированному времени

        private void FixedUpdate()
        {
            Tick();
        }

        private void Tick()
        {
            if (_isTimeRun.Value)
            {
                DeltaTime = Time.deltaTime;
                TimeHasTicked?.Invoke();
            }
            else
            {
                DeltaTime = 0;
            }
        }

        public void StopTime()
        {
            _isTimeRun.Value = false;
        }

        public void RunTime()
        {
            _isTimeRun.Value = true;
        }

        public void PauseTime(float time)
        {
            _isTimeRun.Value = false;

            if (_timer != null)
                StopCoroutine(_timer);

            _timer = StartCoroutine(Timer(time));

        }

        IEnumerator Timer(float pauseTime)
        {
            _time = 0;

            while (_time < pauseTime)
            {
                yield return null;
                _time += Time.deltaTime;
            }

            _timer = null;
            _isTimeRun.Value = true;
        }
    }
}
