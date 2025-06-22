using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
    Enemy_Skeleton enemy_Skeleton => GetComponentInParent<Enemy_Skeleton>();
    private void AnimationTrigger()
    {
        enemy_Skeleton.AnimationFinishTrigger();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy_Skeleton.attackCheck.position, enemy_Skeleton.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().Damage();
            }
        }
    }
    private void OpenCounterWindow() => enemy_Skeleton.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy_Skeleton.CloseCounterAttackWindow();
}
