using R3;
using UnityEngine;

namespace Game.UI
{
    public class UIGameplayRootBinder : MonoBehaviour
    {
        private Subject<Unit> _exitSceneSignalSubj;     // Тип объекта в момент подписки на который не срабатывает событие(Объект сигнала)

        public void OnGoToMainMenuButtonClick()
        {
            _exitSceneSignalSubj?.OnNext(Unit.Default);     // Вызываем срабатывание события, передаем туда "ничего"
        }

        public void Bind(Subject<Unit> exitSceneSignalSubj) // Передаем объект сигнала извне, который не принимает значений
        {
            _exitSceneSignalSubj = exitSceneSignalSubj;
        }
    }
}