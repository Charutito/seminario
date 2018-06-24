using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Siren : MonoBehaviour {
    public Renderer blue;
    public float blueLevel;
    public Renderer red;
    public float redLevel;

    public void Start()
    {
        blue.material.SetFloat("_fresnelbiasss", blueLevel);
        red.material.SetFloat("_fresnelbiasss", redLevel);
    }
    private void Update()
    {
        blue.material.SetFloat("_fresnelbiasss", blueLevel);
        red.material.SetFloat("_fresnelbiasss", redLevel);

    }
}
