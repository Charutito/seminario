using UnityEngine;

namespace GameUtils
{
    public class DestroyIfNotDebug : MonoBehaviour
    {
    #if !DEBUG
        private void Awake()
        {
            Destroy(gameObject);
        }
    #endif
    }
}