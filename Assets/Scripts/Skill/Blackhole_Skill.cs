using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] GameObject blackholePrefab;

    [SerializeField] private float maxSize = 15f;
    [SerializeField] private float growSpeed = 1f;
    [SerializeField] private float shrinkSpeed = 1.5f;
    [SerializeField] private int amountOfAttack = 5;
    [SerializeField] private float cloneAttackCooldown = 0.4f;
    [SerializeField] private float blackholeDuration = 4f;


    Blackhole_Skill_Controller currentBlackhole;

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }
    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, quaternion.identity);

        currentBlackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();
        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttack, cloneAttackCooldown, blackholeDuration);

    }
    public bool CompletedSkill()
    {
        if(!currentBlackhole)
            return false;

        if(currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }
        return false;
    }

}
