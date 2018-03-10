using BattleSystem;
using EZCameraShake;
using Metadata;
using UnityEngine;
using Util;

namespace Entities
{
    [RequireComponent(typeof(EntityMove))]
    public class EntityAttacker : MonoBehaviour
    {
        #region Local Vars
        [SerializeField] private ColliderObserver attackArea;

        [SerializeField] private float heavyAttackRadious = 5f;

        [Header("Heavy Attack")]
        [SerializeField] private float h_magn = 1;
        [SerializeField] private float h_rough = 1;
        [SerializeField] private float h_fadeIn = 0.1f;
        [SerializeField] private float h_fadeOut = 2f;
        [SerializeField] private LayerMask hitLayers;
        [SerializeField] private DamageType h_damageType = DamageType.Unknown;

        private Entity _entity;
        private EntityMove _entityMove;
        #endregion


        #region Light Attack
        public void LightAttack_Start()
        {
            //LookToMouse();
        }

        public void LightAttack_Hit()
        {
            attackArea.gameObject.SetActive(true);
            attackArea.TriggerEnter += LightAttack_Damage;

            FrameUtil.AtEndOfFrame(() => 
            {
                attackArea.TriggerEnter -= LightAttack_Damage;
                attackArea.gameObject.SetActive(false);
            });
        }

        private void LightAttack_Damage(Collider collider)
        {
            var damageable = collider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage((int)_entity.Stats.Damage.Min);
            }
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
            HeavyAttack_Damage();
        }

        private void HeavyAttack_Damage()
        {
            var colliders = Physics.OverlapSphere(attackArea.transform.position, heavyAttackRadious, hitLayers);
            foreach (var collider in colliders)
            {
                var damageable = collider.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.TakeDamage((int)_entity.Stats.Damage.Max, h_damageType);
                }
            }
        }
        #endregion

        private void Start()
        {
            _entity = GetComponent<Entity>();
            _entityMove = GetComponent<EntityMove>();
        }
    }
}
