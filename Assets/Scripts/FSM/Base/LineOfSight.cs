using UnityEngine;
using System.Collections;
using Entities;
using GameUtils;
using Managers;
using Metadata;

namespace Steering
{
    public class LineOfSight : MonoBehaviour
    {
        #region Properties
        public Transform Target { get { return target.transform; } }
        public bool TargetInSight { get { return targetInSight; } }
        #endregion


        #region Inspector Vars
        public Entity target;
        public float viewAngle = 5f;
        public float viewDistance = 10f;
        public float detectionDistance = 4f;
        #endregion


        #region Local Vars
        [SerializeField] private bool targetInSight = false;
        #endregion


        #region Public Functions
        public void Configure(float viewAngle, float viewDistance)
        {
            this.viewAngle = viewAngle;
            this.viewDistance = viewDistance;
        }
        #endregion


        #region Private Functions
        private void Start()
        {
            target = GameManager.Instance.Character;
        }

        private void Update()
        {
            if (target == null)
            {
                targetInSight = false;
                return;
            }

            var dirToTarget = target.transform.position - transform.position;

            var angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

            var sqrDistanceToTarget = (this.transform.position - target.transform.position).sqrMagnitude;

            targetInSight = (sqrDistanceToTarget <= detectionDistance * detectionDistance) ||
                            (angleToTarget <= viewAngle) && (sqrDistanceToTarget <= viewDistance * viewDistance);

            /*targetInSight =
            // Verifica el angulo de vision
            angleToTarget <= viewAngle &&
            // Verifica la distancia de vision
            sqrDistanceToTarget <= viewDistance * viewDistance &&
            // Verifica si hay obstaculos en el medio
            !Physics.Raycast(
                transform.position,
                dirToTarget,
                out rch,
                Mathf.Sqrt(sqrDistanceToTarget),
                layerMask
            );*/
        }

        private void OnDrawGizmosSelected()
        {
            // Dibujamos una línea desde el NPC hasta el enemigo.
            // Va a ser de color verde si lo esta viendo, roja sino.



            if (target != null && targetInSight)
            {
                //Gizmos.color = targetInSight ? Color.green : Color.red;
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, target.transform.position);
            }

            //Dibujamos los límites del campo de visión.
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, viewDistance);

            // Limite del campo de deteccion automatica
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionDistance);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + (transform.forward * viewDistance));

            var rightLimit = Quaternion.AngleAxis(viewAngle, transform.up) * transform.forward;
            Gizmos.DrawLine(transform.position, transform.position + (rightLimit * viewDistance));

            var leftLimit = Quaternion.AngleAxis(-viewAngle, transform.up) * transform.forward;
            Gizmos.DrawLine(transform.position, transform.position + (leftLimit * viewDistance));
        }
        #endregion
    }
}