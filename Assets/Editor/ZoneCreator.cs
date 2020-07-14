using BattleSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ZoneCreator : EditorWindow
{
    private const string AssetsMenuPath = "Assets/Create/Game/Create New Zone";
    private const string GameObjectMenuPath = "GameObject/Game/Create New Zone";
    private const string ZONE_SOUND_PATH = "Assets/GameData/SoundEvents/Environment/ZoneAlert.asset";

    private static Vector2 v = Vector2.zero;
    private static List<GameObject> objs = new List<GameObject>();

    private SimpleAudioEvent defaultAudio;

    [MenuItem("Game/Entities/Zone %&z", false)]
    static void Init()
    {
        ZoneCreator window = (ZoneCreator)EditorWindow.GetWindow(typeof(ZoneCreator));
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox("Select Enemy Prefabs to Instantiate", MessageType.None);

        EditorGUILayout.BeginScrollView(v);

        if (GUILayout.Button("Create an Enemy"))
        {
            objs.Add(default(GameObject));
        }

        for (var i = 0; i < objs.Count; i++)
        {
            objs[i] = (GameObject)EditorGUILayout.ObjectField(objs[i], typeof(GameObject), false);
        }

        if (GUILayout.Button("Remove"))
        {
            if(objs.Count > 0)
                objs.Remove(objs.Last());
        }
        EditorGUILayout.EndScrollView();

        DrawDivider();

        EditorGUILayout.BeginHorizontal();
        defaultAudio = (SimpleAudioEvent)EditorGUILayout.ObjectField(defaultAudio, typeof(SimpleAudioEvent), false);
        defaultAudio = AssetDatabase.LoadAssetAtPath<SimpleAudioEvent>(ZONE_SOUND_PATH);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Build Zone"))
        {
            CreateCustomGameObject();
        }
    }

    private static void DrawDivider()
    {
        GUILayout.Box(string.Empty, GUILayout.ExpandWidth(true), GUILayout.Height(2.0f));
    }

    [MenuItem(AssetsMenuPath)]
    [MenuItem(GameObjectMenuPath, false)]
    public static void CreateCustomGameObject()
    {
        var go = new GameObject("New Zone");

        var zoneCollider = go.AddComponent<BoxCollider>();
        zoneCollider.isTrigger = true;

        var zoneController = go.AddComponent<ZoneController>();

        zoneController.ActionDelay.minValue = 1;
        zoneController.ActionDelay.maxValue = 1;
        zoneController.EntitiesToAttack.minValue = 1;
        zoneController.EntitiesToAttack.maxValue = 1;

        var audioEvent = AssetDatabase.LoadAssetAtPath<SimpleAudioEvent>(ZONE_SOUND_PATH);

        if (audioEvent != null)
        {
            zoneController.InitSound = audioEvent;
        }
        else
        {
            Debug.LogWarning("Can't reference Zone sound at : " + ZONE_SOUND_PATH);
        }

        int sumPos = 0;
        foreach (var item in objs)
        {
            GameObject enemy = Instantiate(item, go.transform);
            enemy.transform.position = new Vector3(++sumPos, 0, 0);
        }

        //GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}
