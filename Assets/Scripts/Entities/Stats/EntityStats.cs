using System;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class EntityStats
    {
        #region Properties
        public Stat Health { get { return health; } }
        public Stat MoveSpeed { get { return movementSpeed; } }
        #endregion

        #region Local Vars
        [SerializeField] private Stat health;
        [SerializeField] private Stat movementSpeed;
        #endregion
    }
}
