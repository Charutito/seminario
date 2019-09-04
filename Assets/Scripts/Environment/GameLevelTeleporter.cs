using Managers;
using UnityEngine;

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
        [UnityEditor.MenuItem("Game/Environment/Teleport", false)]
        public static void CreateCustomGameObject(UnityEditor.MenuCommand menuCommand)
        {
            var go = new GameObject("New Teleport");
            go.AddComponent<GameLevelTeleporter>();
            
            var collider = go.AddComponent<BoxCollider>();
            collider.isTrigger = true;

            UnityEditor.GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            
            // Register the creation in the undo system
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            UnityEditor.Selection.activeObject = go;
        }
    }
#endif
}