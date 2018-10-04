using Environment;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    public class QuestTrigger : MonoBehaviour
    {
        public QuestDefinition QuestToTrigger;
        public QuestObjective QuestObjectiveToComplete;

        public UnityEvent OnTrigger;
    
        private bool _isUsed;

        public void Trigger()
        {
            if (_isUsed) return;
            
            _isUsed = true;

            if (QuestToTrigger != null)
            {
                QuestManager.Instance.StartQuest(QuestToTrigger);
            }
            else if (QuestObjectiveToComplete != null)
            {
                QuestManager.Instance.UpdateQuestProgress(QuestObjectiveToComplete);
            }

            if (OnTrigger != null)
            {
                OnTrigger.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isUsed && other.CompareTag(Metadata.Tags.PLAYER))
            {
                Trigger();
            }
        }
    }
    
#if UNITY_EDITOR
    public static class QuestTriggerCreator
    {
        [MenuItem("Game/Environment/Quest Trigger", false)]
        public static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            var go = new GameObject("New Quest Trigger");
            go.AddComponent<QuestTrigger>();
            
            var collider = go.AddComponent<BoxCollider>();
            collider.isTrigger = true;

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Created " + go.name);
            Selection.activeObject = go;
        }
    }
#endif
}
