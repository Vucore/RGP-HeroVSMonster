using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    Enemy_Skeleton enemy_Skeleton;
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy_Skeleton) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy_Skeleton = _enemy_Skeleton;
    }
    public override void Enter()
    {
        base.Enter();

        enemy_Skeleton.fx.InvokeRepeating("RedColorBlink", 0, 0.1f);

        stateTimer = enemy_Skeleton.stunDuration;
        rb.velocity = new Vector2(-enemy_Skeleton.facingDir * enemy_Skeleton.stunDirection.x, enemy_Skeleton.stunDirection.y);
    }
    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            stateMachine.ChangeState(enemy_Skeleton.idleState);

    }
    public override void Exit()
    {
        base.Exit();
        enemy_Skeleton.fx.Invoke("CancelRedBlink", 0);
    }
}
