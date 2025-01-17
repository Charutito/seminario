﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Entities
{
    public class EntityMove : MonoBehaviour
    {
        private Entity _entity;

        private Coroutine _moveCoroutine;

        #region Movement
        public void MoveTransform(float x, float z, bool rotate = true)
        {
            var newPosition = transform.position + (new Vector3(x, 0, z).normalized * _entity.Stats.MovementSpeed * Time.deltaTime);

            if (rotate)
            {
                RotateTowards(newPosition);
            }

            transform.position = newPosition;
        }
        
        public void MoveTransform(float x, float z, float speed, bool rotate = true)
        {
            var newPosition = transform.position + (new Vector3(x, 0, z).normalized * speed * Time.deltaTime);

            if (rotate)
            {
                RotateTowards(newPosition);
            }

            transform.position = newPosition;
        }
        
        public void SmoothMoveTransform(Vector3 position, float timeToMove, Action onFinish = null)
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }

            _moveCoroutine = StartCoroutine(MoveToPosition(transform, position, timeToMove, onFinish));
        }

        public void ConstantMoveTransform(Vector3 position,float speed, Action onFinish = null)
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }

            _moveCoroutine = StartCoroutine(MoveToPositionConstantly(transform, position, speed, onFinish));
        }
        #endregion

        #region NavMesh Movement
        public void MoveAgent(Vector3 dest)
        {
            _entity.Agent.SetDestination(dest);
        }

        public bool HasAgentArrived()
        {
            if (!_entity.Agent.pathPending && (_entity.Agent.remainingDistance <= _entity.Agent.stoppingDistance))
            {
                return (!_entity.Agent.hasPath || _entity.Agent.velocity.sqrMagnitude <= 0f);
            }

            return false;
        }

        public bool CanReachPosition(Vector3 position)
        {
            var path = new NavMeshPath();
            
            if (NavMesh.CalculatePath(transform.position, position, NavMesh.AllAreas, path))
            {
                return path.status != NavMeshPathStatus.PathPartial;
            }

            return false;
        }

        #endregion


        #region Rotation
        public void RotateTowards(Vector3 target, float rotationSpeed = 10f)
        {
            var direction = (target - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        public void RotateInstant(Vector3 target)
        {
            transform.LookAt(new Vector3(target.x, transform.position.y, target.z), Vector3.up);
        }
        #endregion

        private void Start()
        {
            _entity = GetComponent<Entity>();
        }
        
        public static IEnumerator MoveToPosition(Transform target, Vector3 position, float timeToMove, Action onFinish = null)
        {
            var currentPos = target.position;
            var t = 0f;
            
            while (t < 1)
            {
                t += Time.deltaTime / timeToMove;
                target.position = Vector3.Lerp(currentPos, position, t);
                yield return null;
            }

            if (onFinish != null) onFinish();
        }

        private static IEnumerator MoveToPositionConstantly(Transform target, Vector3 position, float speed, Action onFinish = null)
        {
            var currentPos = target.position;
            var t = 0f;

            while (t < 1)
            {
                t += Time.deltaTime * speed;
                target.position = Vector3.Lerp(currentPos, position, t);
                yield return null;
            }

            if (onFinish != null) onFinish();
        }

        // Animation Crap
        public void FootR() { }
        public void FootL() { }
    }
}
