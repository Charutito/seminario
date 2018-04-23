using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class ChargedTargetElements : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
                var charged = other.GetComponent<ChargedEnemy>();
                if (charged != null)
                {
                    Destroy(gameObject);
                }
            }
        }

    }
}
