using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Enemy_Skeleton enemy_Skeleton;
    protected Transform player;
    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy_Skeleton) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if(enemy_Skeleton.IsPlayerDetected() || Vector2.Distance(player.position, enemy_Skeleton.transform.position) < enemy_Skeleton.attackDistance)
            stateMachine.ChangeState(enemy_Skeleton.battleState);
    
    }
    public override void Exit()
    {
        base.Exit();
    }
}
