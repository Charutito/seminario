using System;
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
        public bool Cleared { get; set; }
        public CharacterEntity Target { get; protected set; }
        public int EnemiesLeft { get { return entities.Count; } }
        public AudioSource audio;
    
        [Header("Attack Delay")]
        public float minAttackDelay = 2f;
        public float maxAttackDelay = 5f;

        [Header("Entities To pick")]
        public int minEntitiesToAttack = 1;
        public int maxEntitiesToAttack = 2;
        
        [SerializeField] private List<BehaviourWeight> entityActions;
        [SerializeField] private List<GroupEntity> entities;
        [SerializeField] private List<GameObject> doors;

        private ZoneFSM fsm;

        // Deberia tener un random para ver si pega uno o otro
        public void ExecuteAttack()
        {
            var entitiesToAttack = entities
                                    .Where(e => e.CurrentAction == GroupAction.Stalking)
                                    .OrderBy(e => Vector3.Distance(Target.transform.position, e.transform.position))
                                    .Take(UnityEngine.Random.Range(minEntitiesToAttack, maxEntitiesToAttack));

            foreach (var entityToAttack in entitiesToAttack)
            {
                entityToAttack.CurrentAction = RandomHelper.Select(entityActions).targetAction;
            }
        }

        public void PrepareEntities()
        {
            foreach (var entity in entities)
            {
                entity.CurrentAction = GroupAction.Stalking;
                if (entity.Target == null) entity.Target = GameManager.Instance.Character;
                entity.OnDeath += OnEntityDie;
            }
        }

        public void ClearZone()
        {
            Cleared = true;
            ToggleDoors(false);
            Destroy(gameObject, 5);
        }

        private void ToggleDoors(bool state)
        {
            foreach (var door in doors)
            {
                door.SetActive(state);
            }
        }

        private void OnEntityDie(Entity entity)
        {
            entities.Remove((GroupEntity)entity);
            entity.OnDeath -= OnEntityDie;
            
            if (entities.Count <= 0)
            {
                ClearZone();
            }
        }

        private void Awake()
        {
            fsm = new ZoneFSM(this);

            ToggleDoors(false);
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
                if(audio != null) audio.Play();
                ToggleDoors(true);
                fsm.PlayerEnter();
            }
        }
    }
}