using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private Enemy_Skeleton enemy_Skeleton;
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy_Skeleton) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy_Skeleton = _enemy_Skeleton;
    }
    public override void Enter()
    {
        base.Enter();
      
    }
    public override void Update()
    {
        base.Update();

        enemy_Skeleton.SetVelocity(0, 0);
        
        if(triggerCalled)
            stateMachine.ChangeState(enemy_Skeleton.battleState);

    
    }
    public override void Exit()
    {
        base.Exit();
        enemy_Skeleton.lastTimeAttack = Time.time;
    }
}
