namespace Syacapachi.Manager {
    using UnityEngine;

    public class ManagerLocator : MonoBehaviour
    {
        public static ManagerLocator Instance { get; private set; }
        [field: SerializeField] public AudioManager AudioManager { get; private set; }
        [field: SerializeField] public GameManager GameManager { get; private set; }
        [field: SerializeField] public MissonManager MissonManager { get; private set; }
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}