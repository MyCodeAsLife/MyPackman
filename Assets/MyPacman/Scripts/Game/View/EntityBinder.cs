using UnityEngine;

namespace MyPacman
{
    public abstract class EntityBinder : MonoBehaviour
    {
        public virtual void Bind(EntityViewModel viewModel)
        {
            transform.position = viewModel.Position.CurrentValue;
        }
    }
}
