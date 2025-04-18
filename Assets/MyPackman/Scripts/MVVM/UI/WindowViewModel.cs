using R3;
using System;

namespace MVVM.UI
{
    public abstract class WindowViewModel : IDisposable
    {
        public Observable<WindowViewModel> CloseRequested => _closeRequested;
        // abstract � ����������� ���� ��������� ����������� �������������� ������ ����
        public abstract string Id { get; }      // ��� ����� ���� � ��������

        private readonly Subject<WindowViewModel> _closeRequested = new();

        public void RequestClose()      // ������ �� �������� ����, ��������� ����
        {
            _closeRequested.OnNext(this);
        }

        public void Dispose() { }
    }
}