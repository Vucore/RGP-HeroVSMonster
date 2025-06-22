using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_Hotkey_Controller : MonoBehaviour
{
   private SpriteRenderer sr;
   private KeyCode myKeyCode;
   private TextMeshProUGUI myText;
   private Blackhole_Skill_Controller blackHole;
   private Transform enemyTranform;
   public void SetupKeyCode(KeyCode _newKeyCode, Blackhole_Skill_Controller _blackHole, Transform _enemyTranform)
   {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myKeyCode = _newKeyCode;
        myText.text = myKeyCode.ToString();

        blackHole = _blackHole;
        enemyTranform = _enemyTranform;
   }
   private void Update() 
   {
        if(Input.GetKeyDown(myKeyCode))
        {
            blackHole.AddEnemyToList(enemyTranform);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
   }
}
