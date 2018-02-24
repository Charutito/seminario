using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(EntityMove))]
    public class EntityAttacker : MonoBehaviour
    {
        private EntityMove _entityMove;

        private void Start()
        {
            _entityMove = GetComponent<EntityMove>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                {
                    _entityMove.RotateInstant(hit.point);
                }
            }
        }
    }
}

