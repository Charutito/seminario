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
}
