using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Entities
{
    public class VfxManager : MonoBehaviour
    {
        public GameObject dashPArts;
        public Transform dashPosition;
        
        public void Dash()
        {
            Instantiate(dashPArts, dashPosition.position, dashPosition.rotation);
        }
    }
}

