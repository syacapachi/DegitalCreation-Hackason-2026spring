namespace Syacapachi.Manager {
    using UnityEngine;

    public class ManagerLocator : MonoBehaviour
    {
        public static ManagerLocator Instance;
        [field: SerializeField] public AudioManager AudioManager { get; private set; }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}