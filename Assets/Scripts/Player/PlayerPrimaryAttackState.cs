using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float timeLastAttacked;
    private float timeAttackReset = 2f;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //xInput = 0;
        
        if(comboCounter > 2 || Time.time > timeLastAttacked + timeAttackReset)
            comboCounter = 0;
        
        stateTimer = 0.1f;

        float attackDir = player.facingDir;
        if(xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
    }
    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            player.SetVelocity(0, rb.velocity.y);

        player.animator.SetInteger("comboCounter", comboCounter);
        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        
        comboCounter++;
        timeLastAttacked = Time.time;

        player.StartCoroutine("BusyFor", 0.15f);
    }
}
