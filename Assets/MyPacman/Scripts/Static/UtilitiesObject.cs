using UnityEngine;

namespace MyPacman
{
    public sealed class UtilitiesObject : MonoBehaviour
    {
        private static GameObject _instance;

        public static GameObject Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("[Utilities Manager]");          // Magic
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }
    }
}
