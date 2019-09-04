using UnityEngine;

namespace GameUtils
{
    public class DestroyIfNotGameDemo : MonoBehaviour
    {
    #if !GAME_DEMO && !DEBUG
        private void Awake()
        {
            Destroy(gameObject);
        }
    #endif
    }
}
