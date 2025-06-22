using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.animator.SetBool("SuccessfullCounterAttack", false);

        stateTimer = player.counterAttackDuration;
    }
    public override void Update() {
        base.Update();
        
        player.SetVelocity(0, 0);
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                if(hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    player.animator.SetBool("SuccessfullCounterAttack", true);
                }
            }
        }

        if(stateTimer < 0 || triggerCalled)
            player.stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
