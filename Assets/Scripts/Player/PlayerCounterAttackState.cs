using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        player.animator.SetBool("SuccessfullCounterAttack", false);

        stateTimer = player.counterAttackDuration;
    }
    public override void Update() {
        base.Update();
        
        player.SetVelocity(0, 0);
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    player.animator.SetBool("SuccessfullCounterAttack", true);
                    if (canCreateClone)
                    { 
                        canCreateClone = false;
                        player.skill.clone_Skill.CreateCloneOnCounterAttack(hit.transform, new Vector3(2 * player.facingDir, 0));
                    }
                }
            }
        }

        if(stateTimer < 0 || triggerCalled)
            player.stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
