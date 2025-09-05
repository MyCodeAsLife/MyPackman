using System;
using System.Collections;
using UnityEngine;

namespace MyPacman
{
    public class TimeService : MonoBehaviour
    {
        public bool IsTimeRun { get; private set; } = true;
        public float DeltaTime { get; private set; }        // Зачем это? к томуж привязанное к фиксированному времени

        private float _time;
        private Coroutine _timer = null;

        public event Action TimeHasTicked;

        private void FixedUpdate()
        {
            Tick();
        }

        private void Tick()
        {
            if (IsTimeRun)
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
            IsTimeRun = false;
        }

        public void RunTime()
        {
            IsTimeRun = true;
        }

        public void PauseTime(float time)
        {
            IsTimeRun = false;

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
            IsTimeRun = true;
        }
    }
}
