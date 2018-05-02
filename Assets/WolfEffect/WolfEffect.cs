using UnityEngine;
using System.Collections;

public class WolfEffect : MonoBehaviour
{
    public Renderer rend;
    public Material newMaterialRef;
    public Shader dissolve;
    private bool activeEffect;
    private float shininess = 0.5f;

    void Start()
    {
        if(rend != null)
            rend = GetComponent<Renderer>();

        rend.material = newMaterialRef;
        rend.material.shader = dissolve;
        //rend.material.shader = Shader.Find("_Cutout");
    }

    private void Update()
    {
        shininess -= 0.7f * Time.deltaTime;
        if(shininess >= 0.01f)
        {
            rend.material.SetFloat("_Cutout", shininess);
        }  
    }

    public void BackToStart()
    {
        shininess = 0.5f;
    }
}