using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour, IDamageable
{
    public GameObject[] drop;
    public Transform DropPoint;   
    public GameObject particles;
    public int maxHits;
    public int currentHits;    
    public AudioEvent DestroyAudio;
    public AudioEvent DmgAudio;

    [Header("Events")]
    public UnityEvent OnDestroyEvent;

    private Coroutine _flashCoroutine;

    public void TakeDamage(Damage damage)
    {
        currentHits++;

        if (particles != null)
        {
            var parts = Instantiate(particles, transform.position, transform.rotation);
            Destroy(parts, 1);
        }
        
        if (currentHits >= maxHits)
        {
            if (DestroyAudio != null)
            {
                DestroyAudio.PlayAtPoint(transform.position);
            }
            
            foreach (var item in drop)
            {
                if (item != null&& DropPoint!=null)
                {
                    Instantiate(item, DropPoint.position, DropPoint.rotation);
                }
            }
            
            Destroy(gameObject);
        }
        else
        {
            _flashCoroutine = StartCoroutine("FlashCorroutine");
            
            if (DmgAudio != null)
            {
                DmgAudio.PlayAtPoint(transform.position);
            }
        }
    }

    private void OnDestroy()
    {
        OnDestroyEvent.Invoke();
        
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }
    }

    private IEnumerator FlashCorroutine()
    {
        for (int i = 0; i < 2; i++)
        {
            gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.red);
            yield return new WaitForSeconds(0.01f);
            gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.white);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
