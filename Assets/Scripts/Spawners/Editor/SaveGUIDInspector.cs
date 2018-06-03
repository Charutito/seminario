using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using SaveSystem;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(SaveGUID))]
public class SaveGUIDInspector : Editor
{
	private SaveGUID _id;

	private string GetGUID()
	{
		var guid = Guid.NewGuid();
		
		if (!Application.isPlaying)
		{
			EditorSceneManager.MarkSceneDirty(_id.gameObject.scene);
			Undo.RecordObject(_id, "Changed Save GUID");
		}
		
		return guid.ToString();
	}

	public override void OnInspectorGUI()
	{
		_id = (SaveGUID)target;

		if (_id.GameObjectId == "")
		{
			_id.GameObjectId = GetGUID();
		}

		EditorGUILayout.LabelField("Unique Id:", _id.GameObjectId);

		if (GUILayout.Button("Refresh"))
		{
			_id.GameObjectId = GetGUID();
		}
		
		var idCount = Array.ConvertAll(GameObject.FindObjectsOfType(typeof(SaveGUID)), x => x as SaveGUID).Count(t => _id.GameObjectId == t.GameObjectId);
			
		if (idCount > 1)
		{
			EditorGUILayout.HelpBox("Warning Duplicate GUID Detected in Scene", MessageType.Warning);
		}
	}
}