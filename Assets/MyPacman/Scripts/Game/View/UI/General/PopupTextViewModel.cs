using R3;
using UnityEngine;

namespace MyPacman
{
    public abstract class PopupTextViewModel : UIViewModel
    {
        public ReadOnlyReactiveProperty<string> Text => _text;
        public ReadOnlyReactiveProperty<Color> TextColor => _textColor;
        public ReadOnlyReactiveProperty<Vector2> Position => _position;

        private readonly ReactiveProperty<string> _text = new();
        private readonly ReactiveProperty<Color> _textColor = new();
        private readonly ReactiveProperty<Vector2> _position = new();

        public void SetText(string text) => _text.OnNext(text);
        public void SetColor(Color textColor) => _textColor.OnNext(textColor);
        public void SetPosition(Vector2 position) => _position.OnNext(position);
    }
}
