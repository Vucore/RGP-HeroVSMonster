using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{
    protected EnemyStateMachine stateMachine;
    protected Rigidbody2D rb;
    protected Enemy enemyBase;
    protected float stateTimer;
    
    private string animBoolName;
    protected bool triggerCalled;
    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemyBase.animator.SetBool(animBoolName, true);
        rb = enemyBase.rb;
    
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Exit()
    {
        enemyBase.animator.SetBool(animBoolName, false);
    }
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
