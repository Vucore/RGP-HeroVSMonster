using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.skill.sword_Skill.DotsActive(true);
    }
    public override void Update() 
    {
        base.Update();  
        player.SetVelocity(0 , 0);
        if(Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(player.transform.position.x > mousePosition.x && player.facingDir == 1)
            player.Flip();
        else if(player.transform.position.x < mousePosition.x && player.facingDir == -1)
            player.Flip();
    }
    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.2f);
    }
}
