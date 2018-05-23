using System.Collections;
using System.Collections.Generic;
using Managers;
using Metadata;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private bool _isLoading;
    
    private void LoadData()
    {
        _isLoading = true;
        
        GameManager.Instance.Character.transform.position = transform.position;
        Log("Game Loaded!");
        Destroy(gameObject);
    }
    
    private void SaveData()
    {
        PlayerPrefs.SetInt(SaveKeys.LastSave, GetInstanceID());
        Log("Game Saved!");
        Destroy(gameObject);
    }

    private void Log(string message)
    {
    #if DEBUG
        Debug.Log(string.Format(FormatedLog.Save, message));
    #endif
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(SaveKeys.LastSave) == GetInstanceID())
        {
            LoadData();
        }
        else if (PlayerPrefs.HasKey(string.Format(SaveKeys.UsedSave, GetInstanceID())))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isLoading) return;
        
        SaveData();
    }
}
