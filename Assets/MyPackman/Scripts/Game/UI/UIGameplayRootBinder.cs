using R3;
using UnityEngine;

namespace Game.UI
{
    public class UIGameplayRootBinder : MonoBehaviour
    {
        // Тип объекта в момент подписки на который не срабатывает событие(Объект сигнала)
        private Subject<Unit> _exitSceneSignalSubj;

        public void OnGoToMainMenuButtonClick()
        {
            // Вызываем срабатывание события, передаем туда "ничего"
            _exitSceneSignalSubj?.OnNext(Unit.Default);
        }

        // Передаем объект сигнала извне, который не принимает значений
        public void Bind(Subject<Unit> exitSceneSignalSubj)
        {
            _exitSceneSignalSubj = exitSceneSignalSubj;
        }
    }
}