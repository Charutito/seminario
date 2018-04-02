﻿using System;
using Entities;
using FSM;
using Managers;
using Metadata;
using System.Collections.Generic;
using System.Linq;
using GameUtils;
using UnityEngine;

namespace BattleSystem
{
    [RequireComponent(typeof(Collider))]
    public class ZoneController : MonoBehaviour
    {
        [Serializable]
        private class BehaviourWeight : IWeighted
        {
            public int Weight { get { return weight; } }
            [SerializeField] public GroupAction targetAction = GroupAction.None;
            [SerializeField] public int weight;
        }

        public bool Initialized { get; set; }
        public CharacterEntity Target { get; protected set; }
        public int EnemiesLeft { get { return entities.Count; } }

        public float minAttackDelay = 2f;
        public float maxAttackDelay = 5f;
        
        [SerializeField] private List<BehaviourWeight> entityActions;
        [SerializeField] private List<GroupEntity> entities;

        private ZoneFSM fsm;

        // Deberia tener un random para ver si pega uno o otro
        public void ExecuteAttack()
        {
            var entityToAttack = entities
                                    .Where(e => e.CurrentAction == GroupAction.Stalking)
                                    .OrderBy(e => Vector3.Distance(Target.transform.position, e.transform.position))
                                    .FirstOrDefault();

            if (entityToAttack != null)
            {
                entityToAttack.CurrentAction = RandomHelper.Select(entityActions).targetAction;
            }
        }

        public void PrepareEntities()
        {
            foreach (var entity in entities)
            {
                entity.CurrentAction = GroupAction.Stalking;
                entity.Target = GameManager.Instance.Character;
                entity.OnDeath += OnEntityDie;
            }
        }

        private void OnEntityDie(Entity entity)
        {
            entities.Remove((GroupEntity)entity);
            entity.OnDeath -= OnEntityDie;
        }

        private void Awake()
        {
            fsm = new ZoneFSM(this);
        }

        private void Start()
        {
            Target = GameManager.Instance.Character;
        }

        private void Update()
        {
            fsm.Update();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Initialized && other.CompareTag(Tags.PLAYER))
            {
                fsm.PlayerEnter();
            }
        }
    }
}