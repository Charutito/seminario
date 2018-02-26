using EZCameraShake;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(EntityMove))]
    public class EntityAttacker : MonoBehaviour
    {
        #region Local Vars
        [Header("Heavy Attack Shake")]
        [SerializeField] private float magn = 1;
        [SerializeField] private float rough = 1;
        [SerializeField] private float fadeIn = 0.1f;
        [SerializeField] private float fadeOut = 2f;

        private EntityMove _entityMove;
        #endregion


        #region Light Attack
        public void LightAttack_Start()
        {
            LookToMouse();
        }

        public void LightAttack_Hit()
        {
            Debug.Log("Light Hit");
        }
        #endregion


        #region Heavy Attack
        public void HeavyAttack_Start()
        {
            LookToMouse();
        }

        public void HeavyAttack_Hit()
        {
            CameraShaker.Instance.ShakeOnce(magn, rough, fadeIn, fadeOut);
        }
        #endregion

        private void LookToMouse()
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                _entityMove.RotateInstant(hit.point);
            }
        }

        private void Start()
        {
            _entityMove = GetComponent<EntityMove>();
        }
    }
}
