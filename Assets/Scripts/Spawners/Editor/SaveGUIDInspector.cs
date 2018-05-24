using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using SaveSystem;

[CustomEditor(typeof(SaveGUID))]
public class SaveGUIDInspector : Editor
{
	private SaveGUID _id;
	
	private void OnEnable()
	{
		_id = (SaveGUID)target;
		
		if (_id.GameObjectId == 0)
		{
			_id.GameObjectId = new System.Random().Next(1000000, 9999999);
		}
		else
		{
			var objects = Array.ConvertAll(GameObject.FindObjectsOfType(typeof(SaveGUID)), x => x as SaveGUID);
			var idCount = objects.Count(t => _id.GameObjectId == t.GameObjectId);
			if (idCount > 1) _id.GameObjectId = new System.Random().Next(1000000, 9999999);
		}
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.LabelField("Unique Id:", _id.GameObjectId.ToString());
	}
}