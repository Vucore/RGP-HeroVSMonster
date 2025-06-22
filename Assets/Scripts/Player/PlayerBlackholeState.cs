using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private bool skillUsed;
    public float flyTime = 0.4f;
    private float defaultGravity;
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        defaultGravity = rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }
    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
            rb.velocity = new Vector2(0, 15);

        if(stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -0.5f);
            if(!skillUsed)
            {
                if(player.skill.blackhole_Skill.CanUseSkill())
                    skillUsed = true;
            }
        }

        if(player.skill.blackhole_Skill.CompletedSkill())
            stateMachine.ChangeState(player.airState);
    }
    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravity;
        player.MakeTransparent(false);
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
