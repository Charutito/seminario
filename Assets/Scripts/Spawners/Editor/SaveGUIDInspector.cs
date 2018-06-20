using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using SaveSystem;
using UnityEditor.SceneManagement;

[CanEditMultipleObjects]
[CustomEditor(typeof(SaveGUID))]
public class SaveGUIDInspector : Editor
{
	private string GetGUID(SaveGUID id)
	{
		var guid = Guid.NewGuid();
		
		if (!Application.isPlaying)
		{
			EditorSceneManager.MarkSceneDirty(id.gameObject.scene);
			Undo.RecordObject(id, "Changed Save GUID");
		}
		
		return guid.ToString();
	}

	public override void OnInspectorGUI()
	{
		var saves = new List<SaveGUID>();
		
		foreach (var current in targets)
		{
			var id = (SaveGUID)current;
			
			if (!saves.Contains(id))
			{
				saves.Add(id);
			}

			if (id.GameObjectId == string.Empty)
			{
				id.GameObjectId = GetGUID(id);
			}

			EditorGUILayout.LabelField("Unique Id:", id.GameObjectId);
		
			var idCount = Array.ConvertAll(FindObjectsOfType(typeof(SaveGUID)), x => x as SaveGUID).Count(t => id.GameObjectId == t.GameObjectId);
			
			if (idCount > 1)
			{
				EditorGUILayout.HelpBox("Warning Duplicate GUID Detected in Scene", MessageType.Warning);
			}
		}
		
		if (GUILayout.Button("Re generate"))
		{
			foreach (var save in saves)
			{
				save.GameObjectId = GetGUID(save);
			}
		}
	}
}