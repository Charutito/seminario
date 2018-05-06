using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpiritController : MonoBehaviour {

    public Renderer rend;
    
    public Material newMaterialRef;
    public Shader dissolve;

    private float shininess;

    bool start;
    bool end;

    public Transform initialPos;
    public GameObject parent;

    void Start () {


        if (rend != null)
            rend = GetComponent<Renderer>();

        rend.materials[1] = newMaterialRef;
        rend.materials[1].shader = dissolve;
    }	
	
	void Update () {

        parent.transform.position += new Vector3(0, 0.5f, 0) * Time.deltaTime;

        if(start && !end)
        {
            if(shininess <= 1)
            {
                shininess += 0.3f;
                rend.materials[1].SetFloat("_TotalOpacity", shininess);
            }
        }
        else if(!start && end)
        {
            if (shininess >= 0.05f)
            {
                shininess -= 0.3f;
                rend.materials[1].SetFloat("_TotalOpacity", shininess);
            }
        }
    }

    public void OnAppear()
    {
        start = true;
        end = false;
    }

    public void OnDisappear()
    {
        start = false;
        end = true;
    }

    public void OnReposition()
    {
        shininess = 0;
        parent.transform.position = initialPos.position;
        StartCoroutine(SetOff(.1f));
    }

    IEnumerator SetOff(float time)
    {
        yield return new WaitForSeconds(time);
        parent.SetActive(false);
    }
}
