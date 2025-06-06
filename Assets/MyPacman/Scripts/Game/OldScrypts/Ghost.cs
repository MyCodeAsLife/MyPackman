using R3;
using UnityEngine;

namespace MyPacman
{
    public class Ghost : MonoBehaviour
    {
        private readonly ReactiveProperty<int> _health = new();

        public ReadOnlyReactiveProperty<int> Health => _health;

        public void Call()
        {
            _health.Value = 0;
            _health.Subscribe(newValue => { Debug.Log($"Health: {newValue}"); });
            _health.OnNext(55);
        }
    }
}