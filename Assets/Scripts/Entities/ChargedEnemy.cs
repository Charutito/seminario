using BattleSystem;
using FSM;
using Steering;
using System;
using GameUtils;
using UnityEngine;

namespace Entities
{
    public class ChargedEnemy : BasicEnemy
    {
        public float ChargeTime;
        public float recoveryTime;
        public event Action OnAttack = delegate { };
        public event Action OnCrash = delegate { };
        public ColliderObserver attackarea;      

        protected override void SetFsm()
        {
            EntityFsm = new ChargedEnemyFSM(this);
            attackarea.TriggerEnter += Crash;
        }

        private void Crash(Collider obj)
        {
            if (obj.gameObject.layer == LayerMask.NameToLayer("Ambient"))
                OnCrash();
        }

        public void Attack()
        {
            if (OnAttack!=null)
            {
                OnAttack();
            }
        }
        

    }
    
}