﻿using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

public class Destructible : MonoBehaviour, IDamageable
{
    public GameObject[] drop;
       
    public GameObject particles;
    
    public int maxHits;
    int currentHits=0;
    public AudioEvent DestroyAudio;
    public void TakeDamage(Damage damage){
        if (currentHits==maxHits)
        {
            foreach (var item in drop)
            {
                Instantiate(item, transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
        else
        {
            StartCoroutine("FlashCorroutine");
            DestroyAudio.PlayAtPoint(transform.position);
            var parts = Instantiate(particles, transform.position, transform.rotation);
            Destroy(parts, 1);
            currentHits++;
        }
        
    }
    IEnumerator FlashCorroutine()
    {
        Debug.Log("hit");
        for (int i = 0; i < 2; i++)
        {

            gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.red);
            yield return new WaitForSeconds(0.01f);
            gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.white);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
