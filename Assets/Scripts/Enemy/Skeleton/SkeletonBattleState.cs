using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Enemy_Skeleton enemy_Skeleton;
    private Transform player;
    private float moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy_Skeleton) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy_Skeleton = _enemy_Skeleton;
    }
    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();

        if(enemy_Skeleton.IsPlayerDetected())
        {
            enemy_Skeleton.SetVelocity(enemy_Skeleton.moveSpeed * enemy_Skeleton.facingDir, rb.velocity.y);
            
            stateTimer = enemy_Skeleton.battleTime;
            if(enemy_Skeleton.IsPlayerDetected().distance < enemy_Skeleton.attackDistance)
            {
                if(CanAttack()) 
                    stateMachine.ChangeState(enemy_Skeleton.attackState);
            }
            return;  ////
        }
        else if(stateTimer < 0 || Vector2.Distance(player.position, enemy_Skeleton.transform.position) > enemy_Skeleton.attackDistance * 6)
            stateMachine.ChangeState(enemy_Skeleton.idleState); 
           
        if(player.position.x > enemy_Skeleton.transform.position.x)
            moveDir = 1;
        else if(player.position.x < enemy_Skeleton.transform.position.x)
            moveDir = -1;

        enemy_Skeleton.SetVelocity(enemy_Skeleton.moveSpeed * moveDir * 2, rb.velocity.y);
    
    }
    public override void Exit()
    {
        base.Exit();
    }
    private bool CanAttack()
    {
        if(Time.time > enemy_Skeleton.lastTimeAttack + enemy_Skeleton.attackCoolDown)
        {
            enemy_Skeleton.lastTimeAttack = Time.time;
            return true;
        }
        return false;
    }
}
