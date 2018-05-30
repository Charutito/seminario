using System;
using Entities;
using FSM;
using Managers;
using Metadata;
using System.Collections.Generic;
using System.Linq;
using GameUtils;
using SaveSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace BattleSystem
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(SaveGUID))]
    public class ZoneController : MonoBehaviour
    {
        [Serializable]
        public class BehaviourWeight : IWeighted
        {
            public int Weight { get { return weight; } }
            public GroupAction targetAction = GroupAction.None;
            public int weight = 10;
        }
        
        #region Properties
        public bool Initialized { get; set; }
        #endregion

        #region Configuration
        [Header("Configuration")]
        [Tooltip("Audio Event to play at zone initialization")]
        public AudioEvent InitSound;
        
        [Tooltip("Random delay between waves")]
        [MinMaxRange(0, 5)]
        public RangedFloat ActionDelay;

        [Tooltip("Max entities to action every wave")]
        [MinMaxRange(1, 20)]
        public RangedFloat EntitiesToAttack;
        
        [Tooltip("Weighted behaviour list to define zone actions in waves.")]
        public List<BehaviourWeight> EntityActions;
        
        [Tooltip("Door GameObjects to show once zone is active.")]
        public List<GameObject> Doors;
        #endregion

        #region Events

        [Header("Events")]
        [Tooltip("This executes once player enters the zone")]
        public UnityEvent OnZoneActivate;
        
        [Tooltip("This executes once the zone is cleared")]
        public UnityEvent OnZoneCleared;
        #endregion
        
        [HideInInspector]
        public readonly List<GroupEntity> Entities = new List<GroupEntity>();
        
        private CharacterEntity _target;
        private SaveGUID _uniqueId;
        private ZoneFSM _fsm;

        public void AddEntity(GroupEntity entity, GroupAction startingAction = GroupAction.None)
        {
            entity.CurrentAction = startingAction;
            entity.CurrentZone = this;

            if (entity.Target == null)
            {
                entity.Target = GameManager.Instance.Character;
            }

            entity.OnDeath += OnEntityDie;

            if (!Entities.Contains(entity))
            {
                Entities.Add(entity);
            }
        }

        public void ExecuteAttack()
        {
            var entitiesToAttack = Entities
                                    .Where(e => e.CurrentAction == GroupAction.Stalking)
                                    .OrderBy(e => Vector3.Distance(_target.transform.position, e.transform.position))
                                    .Take(EntitiesToAttack.GetRandomInt);

            if (EntityActions.Count > 0)
            {
                foreach (var entityToAttack in entitiesToAttack)
                {
                    var result = RandomHelper.Select(EntityActions);
                    
                    if (result != null)
                    {
                        entityToAttack.CurrentAction = result.targetAction;
                    }
                    else
                    {
                    #if DEBUG
                        Debug.LogWarning("The zone " + gameObject.name + " has a null EntityAction. Please, check the Target Action and Weight.", this);
                    #endif
                    }
                }
            }
            else
            {
            #if DEBUG
                Debug.LogWarning("The zone " + gameObject.name + " has no EntityActions. The entities are frozen.", this);
            #endif
            }
        }
        
        private void ClearZone()
        {
            OnZoneCleared.Invoke();
            
            ToggleDoors(false);
            
            PlayerPrefs.SetString(string.Format(SaveKeys.Zones, _uniqueId.GameObjectId), "Cleared");
            Destroy(gameObject, 5);
        }

        private void ToggleDoors(bool state)
        {
            foreach (var door in Doors)
            {
                if (door != null)
                {
                    door.SetActive(state);
                }
            }
        }

        private void OnEntityDie(Entity entity)
        {
            var groupEntity = (GroupEntity)entity;
            if (Entities.Contains(groupEntity))
            {
                Entities.Remove(groupEntity);
            }
            
            entity.OnDeath -= OnEntityDie;
            
            if (Entities.Count <= 0)
            {
                ClearZone();
            }
        }

        private void Awake()
        {
            _uniqueId = GetComponent<SaveGUID>();
            
            if (PlayerPrefs.HasKey(string.Format(SaveKeys.Zones, _uniqueId.GameObjectId)))
            {
                Destroy(gameObject);
                return;
            }

            ToggleDoors(false);
            
            _fsm = new ZoneFSM(this);
        }
        

        private void Start()
        {
            _target = GameManager.Instance.Character;
            
            foreach (Transform child in transform)
            {
                var entity = child.GetComponent<GroupEntity>();
                
                if (entity != null)
                {
                    AddEntity(entity);
                }
            }
        }

        private void Update()
        {
            _fsm.Update();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Initialized && other.CompareTag(Tags.PLAYER))
            {
                if (InitSound != null)
                {
                    InitSound.PlayAtPoint(transform.position);
                }
                OnZoneActivate.Invoke();
                ToggleDoors(true);
            }
        }
    }

#if UNITY_EDITOR
    public static class ZoneCreator
    {
        [MenuItem("Akane/Entities/New zone", false)]
        public static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            const string ZONE_SOUND_PATH = "Assets/GameData/SoundEvents/Environment/ZoneAlert.asset";
            
            var go = new GameObject("New Zone");
            go.AddComponent<SaveGUID>();
            
            var zoneCollider = go.AddComponent<BoxCollider>();
            zoneCollider.isTrigger = true;
            
            var zoneController = go.AddComponent<ZoneController>();
            
            zoneController.ActionDelay.minValue = 1;
            zoneController.ActionDelay.maxValue = 1;
            zoneController.EntitiesToAttack.minValue = 1;
            zoneController.EntitiesToAttack.maxValue = 1;
                
            var audioEvent = AssetDatabase.LoadAssetAtPath<SimpleAudioEvent>(ZONE_SOUND_PATH);

            if (audioEvent != null)
            {
                zoneController.InitSound = audioEvent;
            }
            else
            {
                Debug.LogWarning("Can't reference Zone sound at : " + ZONE_SOUND_PATH);
            }

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
#endif
}