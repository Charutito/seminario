﻿using UnityEngine;
using UnityEngine.UI;
using Util;

namespace QuestSystem
{
    public class QuestCanvasItem : MonoBehaviour
    {
        private const string INTRO = "intro";
        private const string OUTRO = "outro";
        
        [SerializeField]
        private Text titleText;
        
        [SerializeField]
        private Text descriptionText;
        
        [SerializeField]
        private Animator animator;
        
        [SerializeField]
        private  QuestObjectiveCanvasItem objectiveItem;
        
        [SerializeField]
        private Transform objectivesContainer;

        private QuestDefinition _itemDefinition;

        public void Setup(QuestDefinition definition)
        {
            if (_itemDefinition != null) return;
            
            _itemDefinition = definition;

            Bind();
            
            SetText();
        }

        private void SetText()
        {
            titleText.text = _itemDefinition.Title;

            var multipleObjectives = _itemDefinition.Objectives.Count > 1;

            if (multipleObjectives)
            {
                descriptionText.gameObject.SetActive(false);

                foreach (var objective in _itemDefinition.Objectives)
                {
                    var objectiveObject = Instantiate(objectiveItem, objectivesContainer);
                    objectiveObject.Setup(objective);
                }
            }
            else
            {
                descriptionText.text = _itemDefinition.Description;
            }
            
            animator.SetTrigger(INTRO);
        }

        private void OnQuestCompleted(QuestDefinition definition)
        {
            if (definition == _itemDefinition)
            {
                Unbind();
                animator.SetTrigger(OUTRO);
                titleText.color = Color.green;
                FrameUtil.AfterDelay(0.5f, () => Destroy(gameObject));
            }
        }
        
        private void OnQuestFailed(QuestDefinition definition)
        {
            if (definition == _itemDefinition)
            {
                Unbind();
                titleText.color = Color.red;
            }
        }

        private void Bind()
        {
            QuestManager.Instance.OnQuestCompleted += OnQuestCompleted;
            QuestManager.Instance.OnQuestFailed += OnQuestFailed;
        }

        private void Unbind()
        {
            QuestManager.Instance.OnQuestCompleted -= OnQuestCompleted;
            QuestManager.Instance.OnQuestFailed -= OnQuestFailed;
        }
    }
}
