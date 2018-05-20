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
        private Coroutine _dashToggle;

        private void Start()
        {
            Character.OnDash += Dash;
        }
        
        private void Dash()
        {
            if (_dashToggle != null)
            {
                StopCoroutine(_dashToggle);
                dashPArts.SetActive(false);
            }
            
            dashPArts.SetActive(true);

            _dashToggle = StartCoroutine("DeactivatePArts");
        }
        
        private IEnumerator DeactivatePArts()
        {
            yield return new WaitForSeconds(0.5f);
            dashPArts.SetActive(false);
            _dashToggle = null;
        }

    }
}

