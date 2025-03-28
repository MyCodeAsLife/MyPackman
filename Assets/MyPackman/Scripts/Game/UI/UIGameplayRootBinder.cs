using R3;
using UnityEngine;

namespace Game.UI
{
    public class UIGameplayRootBinder : MonoBehaviour
    {
        // ��� ������� � ������ �������� �� ������� �� ����������� �������(������ �������)
        private Subject<Unit> _exitSceneSignalSubj;

        public void OnGoToMainMenuButtonClick()
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