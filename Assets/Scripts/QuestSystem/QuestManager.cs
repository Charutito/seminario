using System;
using System.Collections.Generic;
using GameUtils;
using UnityEngine;

namespace QuestSystem
{
    public class QuestManager : SingletonObject<QuestManager>
    {
        #region Events
        
        public event Action<QuestDefinition> OnQuestCompleted = delegate { };
        public event Action<QuestObjective> OnObjectiveCompleted = delegate { };

        public bool IsQuestCompleted
        {
            get
            {
                return (_currentQuest == null) || _currentQuest.Objectives.Count == _completedObjetives.Count;
            }
        }

        #endregion
        
        public Transform QuestContent;
        
        [SerializeField]
        private QuestCanvasItem _simpleQuestPrefab;
        
        private QuestDefinition _currentQuest;
        private List<QuestObjective> _completedObjetives;
        
        public void StartQuest(QuestDefinition definition)
        {
            _currentQuest = definition;
            _completedObjetives = new List<QuestObjective>();
            
            Debug.Log("Starting quest: " + definition.Title);
            
            var questPrefab = Instantiate(_simpleQuestPrefab, QuestContent);
            questPrefab.Setup(definition);
        }

        public void UpdateQuestProgress(QuestObjective objective)
        {
            if (_currentQuest != null && _currentQuest.Objectives.Contains(objective) && !_completedObjetives.Contains(objective))
            {
                Debug.Log("Updated Quest Progress: " + objective.Title);
                _completedObjetives.Add(objective);
                
                OnObjectiveCompleted.Invoke(objective);
                
                if(IsQuestCompleted) CompleteQuest(_currentQuest);
            }
        }
        
        private void CompleteQuest(QuestDefinition definition)
        {
            if (definition == _currentQuest)
            {
                _currentQuest = null;
                _completedObjetives.Clear();
                
                Debug.Log("Quest Complete: " + definition.Title);
                OnQuestCompleted.Invoke(definition);
            }
        }
    }
}