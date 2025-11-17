using R3;
using UnityEngine;

namespace MyPacman
{
    public class PacmanAnimationEvents : MonoBehaviour
    {
        private readonly Subject<Unit> _deadAnimationFinish = new();

        public Observable<Unit> DeadAnimationFinish => _deadAnimationFinish;

        public void OnDeadAnimationFinishied()
        {
            _deadAnimationFinish.OnNext(Unit.Default);
        }
    }
}
