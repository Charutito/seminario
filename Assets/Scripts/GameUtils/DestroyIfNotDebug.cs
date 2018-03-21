using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfNotDebug : MonoBehaviour
{
#if !DEBUG
    private void Awake()
    {
        Destroy(gameObject);
    }
#endif
}
