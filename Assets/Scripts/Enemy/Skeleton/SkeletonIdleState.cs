using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy_Skeleton) : base(_enemyBase, _stateMachine, _animBoolName, _enemy_Skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy_Skeleton.idleTime;
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer < 0f)
            stateMachine.ChangeState(enemy_Skeleton.moveState);

    }
    public override void Exit()
    {
        base.Exit();
    }
}
