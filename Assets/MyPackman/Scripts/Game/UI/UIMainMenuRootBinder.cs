using R3;
using UnityEngine;

namespace Game.UI
{
    public class UIMainMenuRootBinder : MonoBehaviour
    {
        // ��� ������� � ������ �������� �� ������� �� ����������� �������(������ �������)
        private Subject<Unit> _exitSceneSignalSubj;

        public void OnGoToGameplayButtonClick()
        {
            // �������� ������������ �������, �������� ���� "������"
            _exitSceneSignalSubj?.OnNext(Unit.Default);
        }

        // �������� ������ ������� �����, ������� �� ��������� ��������
        public void Bind(Subject<Unit> exitSceneSignalSubj)
        {
            _exitSceneSignalSubj = exitSceneSignalSubj;
        }
    }
}