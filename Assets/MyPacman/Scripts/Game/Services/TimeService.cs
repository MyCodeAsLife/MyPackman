using System;
using UnityEngine;

namespace MyPacman
{
    public class TimeService : MonoBehaviour
    {
        public bool IsTimeRun { get; private set; } = true;
        public float DeltaTime { get; private set; }        // Зачем это? к томуж привязанное к фиксированному времени

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

        public void PauseTime(float time)       // Пока в разработке за ненадобностью
        {

        }
    }
}
