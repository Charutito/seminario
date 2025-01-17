using System;
using System.Collections.Generic;
using GameUtils;
using UnityEngine;
using Util;

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
        private QuestDefinition _nextQuest;
        
        public void StartQuest(QuestDefinition definition)
        {
            if (_currentQuest != null)
            {
                _nextQuest = definition;
                CompleteQuest(_currentQuest);
                return;
            }

            _currentQuest = definition;
            _nextQuest = definition.NextQuest;
            _completedObjetives = new List<QuestObjective>();
            
            var questPrefab = Instantiate(_simpleQuestPrefab, QuestContent);
            questPrefab.Setup(definition);
        }

        public void UpdateQuestProgress(QuestObjective objective)
        {
            if (_currentQuest != null && _currentQuest.Objectives.Contains(objective) && !_completedObjetives.Contains(objective))
            {
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
                
                OnQuestCompleted.Invoke(definition);

                if (_nextQuest != null)
                {
                    FrameUtil.AfterDelay(0.5f, () => StartQuest(_nextQuest));
                }
            }
        }
    }
}