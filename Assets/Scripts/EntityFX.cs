using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    [Header("Flash")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float hitDuration;
    private Material originalMa;
    private void Start() 
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        originalMa = sr.material;
    }

    private IEnumerator FlashFx()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(hitDuration);
        sr.material = originalMa;
    }
    private void RedColorBlink()
    {
        if(sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;  
        
    }
    private void CancelRedBlink()
    {
        CancelInvoke("RedColorBlink");
        sr.color = Color.white;
    }
}
