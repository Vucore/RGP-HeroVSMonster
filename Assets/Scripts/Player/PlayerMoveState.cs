using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        // if(player.IsWallDetected() && xInput == player.facingDir)
        //     return;

        if(xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
