using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float colorLoosingSpeed;
    [SerializeField] private float attackCheckRadius;
    [SerializeField] private float checkEnemyRadius;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private Transform center;
    private Transform closestEnemy;
    private Animator anim;
    private float cloneTimer;
    private SpriteRenderer sr;
    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update() {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer < 0) 
        {
            sr.color = new Color(1, 1, 1, sr.color.a - colorLoosingSpeed * Time.deltaTime);
            if(sr.color.a <=0)
                Destroy(gameObject);
        }
    }
    public void SetupClone(Transform _newTranform, float _cloneDuration, bool _canAttack, Vector3 _offset)
    {
        if(_canAttack)
        {
            anim.SetInteger("AttackCount", Random.Range(1,3));
            transform.position = _newTranform.position + _offset;
            cloneTimer = _cloneDuration;
        }

        FaceClosestAttack();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center.position, checkEnemyRadius);
    }

    private void FaceClosestAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(center.position, checkEnemyRadius);
        float closestDistanceEnemy = Mathf.Infinity; 
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(center.position, hit.transform.position);
                if(distanceToEnemy < closestDistanceEnemy)
                {
                    closestDistanceEnemy = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        if(closestEnemy != null)
        {
            if(closestEnemy.position.x < center.position.x)
                transform.Rotate(0, 180, 0);
        }
    }
}