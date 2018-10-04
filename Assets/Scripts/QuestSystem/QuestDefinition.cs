using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(menuName = "Game/QuestSystem/New Quest")]
    public class QuestDefinition : ScriptableObject
    {
        public string Title;
        public string Description;
        public List<QuestObjective> Objectives;
        public QuestDefinition NextQuest;
    }
}
