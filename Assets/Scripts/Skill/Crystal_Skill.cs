using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystallPrefab;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float checkEnemyRadius;

    [Header("Multi stacking crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    public override void UseSkill()
    {
        base.UseSkill();
        if (CanUseMultiStacks())
            return;

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
                return;
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone_Skill.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystallPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestAttack(currentCrystal.transform, checkEnemyRadius));
    }
    public void CurrentCrystalChooseRandomTarget() => currentCrystal?.GetComponent<Crystal_Skill_Controller>()?.ChooseRandomEnemy();
    private void OnDrawGizmos()
    {
        if (currentCrystal == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(currentCrystal.transform.position, checkEnemyRadius);
    }
    private bool CanUseMultiStacks()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count >= 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }

                coolDown = 0f;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                crystalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestAttack(newCrystal.transform, checkEnemyRadius));
                if (crystalLeft.Count <= 0)
                {
                    coolDown = multiStackCooldown;
                    RefilCrystal();
                }
                return true;
            }
        }
        return false;
    }
    private void RefilCrystal()
    {

        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystallPrefab);
        }
    }
    private void ResetAbility()
    {
        if (coolDownTimer > 0)
            return;
        coolDownTimer = multiStackCooldown;
        RefilCrystal();
    }
}
