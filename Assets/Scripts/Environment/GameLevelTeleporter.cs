using Managers;
using SaveSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Util;

namespace Environment
{
    public class GameLevelTeleporter : MonoBehaviour
    {
        public string SceneName;
        
        [SerializeField]
        private bool _active = true;

        public bool IsActive {
            get { return _active; }
            set { _active = value; }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_active) return;
            
            GameManager.Instance.LoadScene(SceneName);
        }
    }
    
#if UNITY_EDITOR
    public static class TeleporterCreator
    {
        [MenuItem("Akane/Environment/Teleport", false)]
        public static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            var go = new GameObject("New Teleport");
            go.AddComponent<GameLevelTeleporter>();
            
            var collider = go.AddComponent<BoxCollider>();
            collider.isTrigger = true;

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
#endif
}