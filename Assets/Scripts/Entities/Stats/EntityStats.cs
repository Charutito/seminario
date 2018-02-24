using System;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class EntityStats
    {
        #region Properties
        public Stat Health { get { return _Health; } }
        public Stat MoveSpeed { get { return _MovementSpeed; } }
        #endregion

        #region Local Vars
        [SerializeField] private Stat _Health;

        [SerializeField] private Stat _MovementSpeed;
        #endregion
    }
}
