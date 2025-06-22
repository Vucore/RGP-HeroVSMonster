using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }
    public override void Update() {
        base.Update();

        // if(player.IsWallDetected())
        // {
        //     stateMachine.ChangeState(player.wallSlideState);
        //     return;
        // }

        if(rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);

        if(xInput != 0 && !player.IsWallDetected())
            player.SetVelocity(xInput * player.moveSpeed * 0.8f, rb.velocity.y);

    
    }
    public override void Exit()
    {
        base.Exit();
    }
}
