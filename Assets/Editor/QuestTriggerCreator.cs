using QuestSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class QuestTriggerCreator : EditorWindow
{
    private const string AssetsMenuPath = "Assets/Create/Quest System/Create New Quest Trigger";
    private const string GameObjectMenuPath = "GameObject/Game/Quest System/Create New Quest Trigger";

    private string objectiveTitle = "";

    private string definitionTitle = "";
    private string definitionDescription = "";

    private static Vector2 v = Vector2.zero;
    private static List<QuestObjective> objs = new List<QuestObjective>();

    [MenuItem("Game/Quest System/Quest Trigger Creator", false)]
    static void Init()
    {
        QuestTriggerCreator window = (QuestTriggerCreator)EditorWindow.GetWindow(typeof(QuestTriggerCreator));
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox("Create a new Quest trigger", MessageType.None);
        
        objectiveTitle = EditorGUILayout.TextField("Objective Title", objectiveTitle);

        if (GUILayout.Button("Create Objetive"))
        {
            objs.Add(CreateObjective(objectiveTitle));
        }

        if (GUILayout.Button("Add Empty Objetive"))
        {
            objs.Add(default(QuestObjective));
        }

        EditorGUILayout.BeginScrollView(v);

        for (var i = 0; i < objs.Count; i++)
        {
            objs[i] = (QuestObjective)EditorGUILayout.ObjectField(objs[i], typeof(QuestObjective), false);
        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Remove Last"))
        {
            if (objs.Count > 0)
                objs.Remove(objs.Last());
        }

        DrawDivider();

        definitionTitle = EditorGUILayout.TextField("Definition Title", definitionTitle);
        definitionDescription = EditorGUILayout.TextField("Definition Description", definitionDescription);

        if (GUILayout.Button("Create Definition"))
        {
            CreateQuestDefinition(definitionTitle, definitionDescription, objs);
        }
    }

    private QuestObjective CreateObjective(string title)
    {
        QuestObjective objective = ScriptableObject.CreateInstance<QuestObjective>();
        objective.Title = title;
        AssetDatabase.CreateAsset(objective, "Assets/GameData/QuestSystem/Objectives/" + title + ".asset");
        return objective;
    }

    private void CreateQuestDefinition(string title, string description, List<QuestObjective> objectives)
    {
        QuestDefinition definition = ScriptableObject.CreateInstance<QuestDefinition>();
        definition.Title = title;
        definition.Description = description;
        definition.Objectives = objectives;
        AssetDatabase.CreateAsset(definition, "Assets/GameData/QuestSystem/Quests/" + title + ".asset");

        var go = new GameObject("New Quest Trigger");
        go.AddComponent<QuestTrigger>();
        go.GetComponent<QuestTrigger>().QuestToTrigger = definition;
        go.GetComponent<QuestTrigger>().QuestObjectiveToComplete = objectives.First();

        var collider = go.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        Undo.RegisterCreatedObjectUndo(go, "Created " + go.name);
        Selection.activeObject = go;
    }

    private static void DrawDivider()
    {
        GUILayout.Box(string.Empty, GUILayout.ExpandWidth(true), GUILayout.Height(2.0f));
    }

    [MenuItem(AssetsMenuPath)]
    [MenuItem(GameObjectMenuPath, false)]
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
