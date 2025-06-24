using UnityEngine;

namespace MyPacman
{
    public class PacmanBinder : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Bind(PackmanViewModel viewModel)
        {
            // Функция/лямбда(подписка) на поворот в сторону движения.
            // Функция/лямбда(подписка) на движение/смену позиции.
        }

        // Получать позицию игрока здесь, она напрямую зависит от коллайдера и rigitbody
        public Vector2 GetPosition() => _rigidbody.position;
    }
}
