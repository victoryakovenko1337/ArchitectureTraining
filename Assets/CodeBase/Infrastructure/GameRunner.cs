using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        public GameBootstrapper BootstrapperPrefab;

        private void Awake()
        {
            GameBootstrapper bootstraper = FindObjectOfType<GameBootstrapper>();

            if(bootstraper == null)
            {
                Instantiate(BootstrapperPrefab);
            }
        }
    }
}