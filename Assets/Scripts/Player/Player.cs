using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    public bool isBusy { get; private set; }
    [Header("Attack")]
    public float counterAttackDuration;
    public Vector2[] attackMovement;
    [Header("Move")]
    public float moveSpeed = 0f;
    public float jumpForce = 0f;
    public float swordReturnImpact = 0f;

    [Header("Dash")]
    public float dashSpeed = 0f;
    public float dashDuration = 0f;
    public float dashDir { get; private set; }


    public SkillManager skill;
    public GameObject sword { get; private set; }

    #region State
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState{ get; private set; }
    public PlayerJumpState jumpState{ get; private set; }
    public PlayerAirState airState{ get; private set; }
    public PlayerDashState dashState{ get; private set; }
    public PlayerWallSlideState wallSlide{ get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }

    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }

    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerBlackholeState blackhole { get; private set; }
    #endregion
    protected override void Awake() 
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");   
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");

        blackhole = new PlayerBlackholeState(this, stateMachine, "Jump");
    }
    protected override void Start()
    {
        base.Start();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        CheckForDashInput();
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    public IEnumerator BusyFor(float _time)
    {
        isBusy = true;
        yield return new WaitForSeconds(_time);
        isBusy = false;
    }
    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }
    private void CheckForDashInput()
    {
        if(IsWallDetected())
            return;

       // dashUsageTimer -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash_Skill.CanUseSkill())
        {
          //  dashUsageTimer = dashCoolDown;
            dashDir = Input.GetAxisRaw("Horizontal");
            if(dashDir == 0)
                dashDir = facingDir;
            
            stateMachine.ChangeState(dashState);
        }
    }
    public override bool IsGroundDetected() => Physics2D.OverlapCircle(groundCheck.position, cirleGroundCheckRadius, whatIsGround);

}

