using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Entities
{
    public class VfxManager : MonoBehaviour
    {
        public CharacterEntity Character;
        public GameObject dashPArts;
        // Use this for initialization
        void Start()
        {
            Character.OnDash += Dash;
        }
        // Update is called once per frame
        void Update()
        {

        }
        private void OnDestroy()
        {

        }
        private void Dash()
        {
            dashPArts.SetActive(true);
            StartCoroutine("DeactivatePArts");
        }
         IEnumerator DeactivatePArts()
        {
            yield return new WaitForSeconds(0.5f);
            dashPArts.SetActive(false);


        }

    }
}

