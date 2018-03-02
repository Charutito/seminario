using EZCameraShake;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(EntityMove))]
    public class EntityAttacker : MonoBehaviour
    {
        #region Local Vars

        [Header("Heavy Attack Shake")]
        [SerializeField] private float l_magn = 1;
        [SerializeField] private float l_rough = 1;
        [SerializeField] private float l_fadeIn = 0.1f;
        [SerializeField] private float l_fadeOut = 2f;

        [Header("Heavy Attack Shake")]
        [SerializeField] private float h_magn = 1;
        [SerializeField] private float h_rough = 1;
        [SerializeField] private float h_fadeIn = 0.1f;
        [SerializeField] private float h_fadeOut = 2f;

        private EntityMove _entityMove;
        #endregion


        #region Light Attack
        public void LightAttack_Start()
        {
            //LookToMouse();
        }

        public void LightAttack_Hit()
        {
            CameraShaker.Instance.ShakeOnce(l_magn, l_rough, l_fadeIn, l_fadeOut);
        }
        #endregion


        #region Heavy Attack
        public void HeavyAttack_Start()
        {
            //LookToMouse();
        }

        public void HeavyAttack_Hit()
        {
            CameraShaker.Instance.ShakeOnce(h_magn, h_rough, h_fadeIn, h_fadeOut);
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
