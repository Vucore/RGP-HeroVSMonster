using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private List<KeyCode> keyCodeList;
    [SerializeField] private GameObject hotKeyPrefabs;
    
    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;

    private int amountOfAttack = 4;
    private float cloneAttackCooldown = 0.4f;
    private bool playerCanDisapear = true;

    private bool canGrow = true;
    private bool canShrink;
    private float blackholeTimer;


    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createHotkey = new List<GameObject>();


    private bool canCreateHotkeys = true;
    private bool canCreateCloneAttack;
    private float cloneAttackTimer;

    public bool playerCanExitState { get; private set; }

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttack, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttack = _amountOfAttack;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;
        if (SkillManager.instance.clone_Skill.createCrystalInsteadOfClone)
        {
            playerCanDisapear = false;
        }
    }

    void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;
        
        if(blackholeTimer <= 0)
        {
            blackholeTimer = Mathf.Infinity;
            if(targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbility();
        }

        if (Input.GetKeyDown(KeyCode.R) && amountOfAttack > 0)
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }

    }

    private void ReleaseCloneAttack()
    {
        if(targets.Count <= 0)
            return;

        DestroyHotkey();
        canCreateCloneAttack = true;
        canCreateHotkeys = false;
        if(playerCanDisapear)
        {
            playerCanDisapear = false;
            PlayerManager.instance.player.MakeTransparent(true);
        }
    }
    private void DestroyHotkey()
    {
        if(createHotkey.Count <= 0)
            return;

        for (int i = 0; i < createHotkey.Count; i++)
        {
            Destroy(createHotkey[i]);
        }
    }
    private void CloneAttackLogic()
    {
        if (cloneAttackTimer <= 0 && canCreateCloneAttack && amountOfAttack > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;
            if (Random.Range(0, 100) > 50)
                xOffset = 1.5f;
            else
                xOffset = -1.5f;

            if (SkillManager.instance.clone_Skill.createCrystalInsteadOfClone)
            {
                SkillManager.instance.crystal_Skill.CreateCrystal();
                SkillManager.instance.crystal_Skill.CurrentCrystalChooseRandomTarget();
            }

            SkillManager.instance.clone_Skill.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));

            amountOfAttack--;


            if (amountOfAttack <= 0)
            {
                Invoke("FinishBlackholeAbility", 1.2f);
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotkey();
        canCreateCloneAttack = false;
        canShrink = true;
        
        playerCanExitState = true;     
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.GetComponent<Enemy>() != null)
        {
            other.GetComponent<Enemy>().FreezeTime(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(!canCreateHotkeys)
            return;

        if(other.GetComponent<Enemy>() != null)
        {
            other.GetComponent<Enemy>().FreezeTime(true);
           // other.GetComponent<Enemy>().rb.constraints = RigidbodyConstraints2D.FreezePosition;////

            CreateHotKey(other);

        }
    }
    
    private void CreateHotKey(Collider2D other)
    {
        if(keyCodeList.Count <= 0)
            return;
        
        // if(canCreateCloneAttack)
        //     return;

        GameObject newHotKey = Instantiate(hotKeyPrefabs, other.transform.position + new Vector3(0, 1.5f), Quaternion.identity);
        createHotkey.Add(newHotKey);
        KeyCode chooseKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chooseKey);

        Blackhole_Hotkey_Controller newHotkeyScript = newHotKey.GetComponent<Blackhole_Hotkey_Controller>();
        newHotkeyScript.SetupKeyCode(chooseKey, this, other.transform);
    }
    public void AddEnemyToList(Transform _enemyTranform)
    {
        targets.Add(_enemyTranform);
    }
}
