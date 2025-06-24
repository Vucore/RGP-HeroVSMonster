using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        stateTimer = player.dashDuration;
        //SkillManager.instance.clone_Skill.CreateClone(player.transform);
        // player.skill.clone_Skill.CreateClone(player.transform, Vector3.zero);
        player.skill.clone_Skill.CreateCloneOnDashStart();

    }
    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);
        
        if(!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        if(stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        player.skill.clone_Skill.CreateCloneOnDashOver();
        player.SetVelocity(0, rb.velocity.y);
    }
}
