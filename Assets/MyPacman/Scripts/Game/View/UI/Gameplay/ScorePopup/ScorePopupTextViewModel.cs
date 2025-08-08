using R3;
using UnityEngine;

namespace MyPacman
{
    // Переименовать
    public class ScorePopupTextViewModel : PopupTextViewModel
    {
        public readonly ReactiveProperty<string> Text = new();
        public readonly ReactiveProperty<Vector2> Position = new();

        //public ScorePopupTextViewModel(int id) => Id = id;
        public override string Id => "PopupText";                                   // Magic
        public void ChangeText(string text) => Text.OnNext(text);
        public void SetPosition(Vector2 position) => Position.OnNext(position);
    }
}
