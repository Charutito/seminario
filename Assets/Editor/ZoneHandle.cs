using BattleSystem;
using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ZoneController))]
public class ZoneHandle : Editor
{
    public Texture BoxTexture;

    void OnSceneGUI()
    {
        ZoneController handle = (ZoneController)target;
        if (handle == null)
        {
            return;
        }
        Handles.BeginGUI();
        //--- Title ---//
        GUIContent titleContent = new GUIContent("Zone Data", BoxTexture);
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontSize = 15;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.red;

        titleStyle.imagePosition = ImagePosition.ImageAbove;
        Vector3 pos = handle.transform.position;
        Vector2 pos2D = HandleUtility.WorldToGUIPoint(pos);
        GUI.Box(new Rect(pos2D.x, pos2D.y, 100, 100), titleContent, titleStyle);

        //--- Enemy Qty ---//
        int enemyQty = handle.GetComponentsInChildren<GroupEntity>().Length;
        GUIContent contentEnemyQty = new GUIContent("Enemies Quantity: " + enemyQty, BoxTexture);
        GUIStyle contentEnemyQtyStyle = new GUIStyle();
        contentEnemyQtyStyle.alignment = TextAnchor.MiddleCenter;
        contentEnemyQtyStyle.fontSize = 15;
        contentEnemyQtyStyle.fontStyle = FontStyle.Bold;
        contentEnemyQtyStyle.normal.textColor = Color.cyan;
        GUI.Box(new Rect(pos2D.x, pos2D.y + 70, 100, 100), contentEnemyQty, contentEnemyQtyStyle);

        //--- Enemy Total Health ---//
        GroupEntity[] enemies = handle.GetComponentsInChildren<GroupEntity>();
        int totalHealth = 0;
        foreach (var item in enemies)
        {
            totalHealth += item.Definition.MaxHealth;
        }
        GUIContent contentEnemyTotalHealth = new GUIContent("Enemies Total Health: " + totalHealth, BoxTexture);
        GUIStyle contentEnemyTotalHealthStyle = new GUIStyle();
        contentEnemyTotalHealthStyle.alignment = TextAnchor.MiddleCenter;
        contentEnemyTotalHealthStyle.fontSize = 15;
        contentEnemyTotalHealthStyle.fontStyle = FontStyle.Bold;
        contentEnemyTotalHealthStyle.normal.textColor = Color.cyan;
        GUI.Box(new Rect(pos2D.x, pos2D.y + 100, 100, 100), contentEnemyTotalHealth, contentEnemyTotalHealthStyle);

        //--- Enemy Light Damage ---//
        int totalLightDamage = 0;
        foreach (var item in enemies)
        {
            totalLightDamage += item.Definition.LightAttackDamage;
        }
        GUIContent contentEnemyLightDamage = new GUIContent("Enemies Light Damage: " + totalLightDamage, BoxTexture);
        GUIStyle contentEnemyLightDamageStyle = new GUIStyle();
        contentEnemyLightDamageStyle.alignment = TextAnchor.MiddleCenter;
        contentEnemyLightDamageStyle.fontSize = 15;
        contentEnemyLightDamageStyle.fontStyle = FontStyle.Bold;
        contentEnemyLightDamageStyle.normal.textColor = Color.cyan;
        GUI.Box(new Rect(pos2D.x, pos2D.y + 130, 100, 100), contentEnemyLightDamage, contentEnemyLightDamageStyle);

        //--- Enemy Special Damage ---//
        int totalSpecialDamage = 0;
        foreach (var item in enemies)
        {
            totalSpecialDamage += item.Definition.SpecialAttackDamage;
        }
        GUIContent contentEnemySpecialDamage = new GUIContent("Enemies Special Damage: " + totalSpecialDamage, BoxTexture);
        GUIStyle contentEnemySpecialDamageStyle = new GUIStyle();
        contentEnemySpecialDamageStyle.alignment = TextAnchor.MiddleCenter;
        contentEnemySpecialDamageStyle.fontSize = 15;
        contentEnemySpecialDamageStyle.fontStyle = FontStyle.Bold;
        contentEnemySpecialDamageStyle.normal.textColor = Color.cyan;
        GUI.Box(new Rect(pos2D.x, pos2D.y + 160, 100, 100), contentEnemySpecialDamage, contentEnemySpecialDamageStyle);


        Handles.EndGUI();




        /*
        ZoneController handle = (ZoneController)target;
        if (handle == null)
        {
            return;
        }

        //Handles.color = Color.blue;
        //Handles.Label(handle.transform.position + Vector3.up * 2, "Zone");

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;

        Handles.BeginGUI();
        Vector3 pos = handle.transform.position;
        Vector2 pos2D = HandleUtility.WorldToGUIPoint(pos);
        GUI.Box(new Rect(pos2D.x, pos2D.y, 100, 100), "A BOX");
        //GUI.Label(new Rect(pos2D.x, pos2D.y, 100, 100), pos.ToString(), style);
        Handles.EndGUI();
        */
    }
}
