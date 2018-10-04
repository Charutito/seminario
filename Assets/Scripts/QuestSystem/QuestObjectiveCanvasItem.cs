using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    public class QuestObjectiveCanvasItem : MonoBehaviour
    {
        private const string TITLE_FORMAT = "{0} {1}";
        private const string MINUS = "-";
        private const string CHECKMARK = "✓";
        
        [SerializeField]
        private Text titleText;
        
        private QuestObjective _itemDefinition;
        
        public void Setup(QuestObjective definition)
        {
            if (_itemDefinition != null) return;
            
            _itemDefinition = definition;

            QuestManager.Instance.OnObjectiveCompleted += OnObjectiveCompleted;
            
            SetText();
        }
        
        private void SetText()
        {
            titleText.text = string.Format(TITLE_FORMAT, MINUS, _itemDefinition.Title);
        }

        private void OnObjectiveCompleted(QuestObjective definition)
        {
            if (definition == _itemDefinition)
            {
                QuestManager.Instance.OnObjectiveCompleted -= OnObjectiveCompleted;
                
                titleText.text = string.Format(TITLE_FORMAT, CHECKMARK, _itemDefinition.Title);
                titleText.color = Color.green;
            }
        }
    }
}
