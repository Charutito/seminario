using System;
using Entities;
using FSM;
using Managers;
using Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using GameUtils;
using SaveSystem;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Events;
using Util;

namespace BattleSystem
{
    public class ZoneController : MonoBehaviour
    {
        
        #region Properties
        public bool Initialized { get; set; }
        #endregion

        #region Configuration
        [Header("Sound")]
        [Tooltip("Audio Event to play at zone initialization")]
        public AudioEvent InitSound;
        
        [Header("Attack")]
        [Tooltip("Random delay between waves")]
        [MinMaxRange(0, 5)]
        public RangedFloat ActionDelay;
        
        [Tooltip("Max entities to action every wave")]
        [MinMaxRange(1, 20)]
        public RangedFloat EntitiesToAttack;

        [Header("Zones")]
        [Tooltip("If true on zone activation it will activate all child zones")]
        public bool ActivateChildZones = false;
        
        [Tooltip("Delay before clear zone gameobject (includes all child gameobjects)")]
        public float DestroyDelay = 5f;
        #endregion

        #region Events

        [Header("Events (in execution order)")]
        [Tooltip("This is executed once zone is instantiated")]
        public ZoneEvent OnZoneSetup;
        
        [Tooltip("This executes once player enters the zone")]
        public ZoneEvent OnZoneActivate;
        
        [Tooltip("This executes once the zone is cleared")]
        public ZoneEvent OnZoneCleared;
        
        [Tooltip("This executes once current zone and all child zones are cleared")]
        public ZoneEvent OnZoneDeactivate;
        #endregion
        
        [HideInInspector]
        public readonly List<GroupEntity> Entities = new List<GroupEntity>();
        [HideInInspector]
        public readonly List<EntitySpawner> Spawners = new List<EntitySpawner>();
        [HideInInspector]
        public readonly List<ZoneController> ChildZones = new List<ZoneController>();
        
        private CharacterEntity _target;
        private ZoneFSM _fsm;

        public void Initialize()
        {
            if (Initialized) return;
            
            if (InitSound != null)
            {
                InitSound.PlayAtPoint(transform.position);
            }
            
            OnZoneActivate.Invoke(this);
        }

        public void AddEntity(GroupEntity entity, GroupAction startingAction = GroupAction.None)
        {
            if (!Entities.Contains(entity))
            {
                Entities.Add(entity);
                
                if (entity.Target == null)
                {
                    entity.Target = GameManager.Instance.Character;
                }
                
                FrameUtil.OnNextFrame(() =>
                {
                    entity.CurrentZone = this;
                    entity.CurrentAction = startingAction;
                });

                entity.OnDeath += OnEntityDie;
            }
        }

        public void AddSpawner(EntitySpawner spawner)
        {
            if (!Spawners.Contains(spawner))
            {
                Spawners.Add(spawner);
                spawner.OnSpawnerCleared.AddListener(OnSpawnerCleared);
            }
        }

        public void AddZone(ZoneController zone)
        {
            if (!ChildZones.Contains(zone))
            {
                ChildZones.Add(zone);
                zone.OnZoneDeactivate.AddListener(OnChildZoneCleared);
            }
        }

        public void ExecuteAttack()
        {
            var entitiesToAttack = Entities
                                    .Where(e => e.CurrentAction == GroupAction.Stalking || e.CurrentAction == GroupAction.None)
                                    .OrderBy(e => Vector3.Distance(_target.transform.position, e.transform.position))
                                    .Take(EntitiesToAttack.GetRandomInt);

            foreach (var entityToAttack in entitiesToAttack)
            {
                entityToAttack.CurrentAction = GroupAction.Attacking;
            }
        }
        
        private void ClearZone()
        {
            OnZoneDeactivate.Invoke(this);
            
            Destroy(gameObject, DestroyDelay);
        }

        private void CheckClearConditions()
        {
            if (Entities.Count <= 0 && Spawners.Count <= 0)
            {
                OnZoneCleared.Invoke(this);

                if (ChildZones.Count <= 0)
                {
                    ClearZone();
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
            
            CheckClearConditions();
        }

        private void OnSpawnerCleared(EntitySpawner spawner)
        {
            if (Spawners.Contains(spawner))
            {
                Spawners.Remove(spawner);
            }

            spawner.OnSpawnerCleared.RemoveListener(OnSpawnerCleared);
            
            CheckClearConditions();
        }
        
        private void OnChildZoneCleared(ZoneController zone)
        {
            if (ChildZones.Contains(zone))
            {
                ChildZones.Remove(zone);
            }

            zone.OnZoneDeactivate.RemoveListener(OnChildZoneCleared);
            
            CheckClearConditions();
        }

        private void SetupZone()
        {
            OnZoneSetup.Invoke(this);
            
            foreach (Transform child in transform)
            {
                var entity = child.GetComponent<GroupEntity>();
                
                if (entity != null)
                {
                    AddEntity(entity);
                    continue;
                }

                var spawner = child.GetComponent<EntitySpawner>();
                
                if (spawner != null)
                {
                    AddSpawner(spawner);
                    continue;
                }
                
                var zone = child.GetComponent<ZoneController>();
                
                if (zone != null)
                {
                    AddZone(zone);
                    continue;
                }
            }
            
            _fsm = new ZoneFSM(this);
        }

        private void Start()
        {
            _target = GameManager.Instance.Character;
            
            SetupZone();
        }

        private void Update()
        {
            _fsm.Update();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Initialized && other.CompareTag(Tags.PLAYER))
            {
                Initialize();
            }
        }
    }
    
    [Serializable]
    public sealed class ZoneEvent : UnityEvent<ZoneController> {}

#if UNITY_EDITOR
    public static class ZoneCreator
    {
        [MenuItem("Akane/Entities/Zone", false)]
        public static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            const string ZONE_SOUND_PATH = "Assets/GameData/SoundEvents/Environment/ZoneAlert.asset";
            
            var go = new GameObject("New Zone");
            
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