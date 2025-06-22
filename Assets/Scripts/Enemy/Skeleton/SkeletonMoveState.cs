using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy_Skeleton) : base(_enemyBase, _stateMachine, _animBoolName, _enemy_Skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        enemy_Skeleton.SetVelocity(enemy_Skeleton.moveSpeed * enemy_Skeleton.facingDir, rb.velocity.y);
        if(!enemy_Skeleton.IsGroundDetected() || enemy_Skeleton.IsWallDetected())
        {
            enemy_Skeleton.Flip();
            enemy_Skeleton.SetVelocity(0,0);
            stateMachine.ChangeState(enemy_Skeleton.idleState);
        }

    }
    public override void Exit()
    {
        base.Exit();
    }
}
