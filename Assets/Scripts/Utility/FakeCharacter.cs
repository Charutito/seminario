using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace Entities
{
    public class FakeCharacter : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {            
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {               
                var charged = other.GetComponent<ChargedEnemy>();
                if(charged != null)
                {
                    charged.Target = GameManager.Instance.Character;
                    Destroy(gameObject);
                }                
            }
        }
    }
}
