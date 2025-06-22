using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
    protected override void Update()
    {
        base.Update();
        // if(Input.GetKey(KeyCode.U))
        //     stateMachine.ChangeState(stunnedState);
    }
    public override bool CanBeStunned()
    {
        if(base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

}
