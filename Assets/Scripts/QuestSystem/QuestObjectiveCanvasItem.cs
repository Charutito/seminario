using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    public class QuestObjectiveCanvasItem : MonoBehaviour
    {
        private const string TITLE_FORMAT = "{0} {1}";
        private const string MINUS = "-";
        private const string CHECKMARK = "✓";
        private const string CROSS = "X";
        
        [SerializeField]
        private Text titleText;
        
        private QuestObjective _itemDefinition;
        
        public void Setup(QuestObjective definition)
        {
            if (_itemDefinition != null) return;
            
            _itemDefinition = definition;
            
            Bind();
            
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
                Unbind();
                
                titleText.text = string.Format(TITLE_FORMAT, CHECKMARK, _itemDefinition.Title);
                titleText.color = Color.green;
            }
        }
        
        private void OnObjectiveFailed(QuestObjective definition)
        {
            if (definition == _itemDefinition)
            {
                Unbind();
                
                titleText.text = string.Format(TITLE_FORMAT, CROSS, "FAILED");
                titleText.color = Color.red;
            }
        }

        private void Bind()
        {
            QuestManager.Instance.OnObjectiveCompleted += OnObjectiveCompleted;
            QuestManager.Instance.OnObjectiveFailed += OnObjectiveFailed;
        }

        private void Unbind()
        {
            QuestManager.Instance.OnObjectiveCompleted -= OnObjectiveCompleted;
            QuestManager.Instance.OnObjectiveFailed -= OnObjectiveFailed;
        }
    }
}
