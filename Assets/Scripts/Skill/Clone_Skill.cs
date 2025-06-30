using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone")]
    [SerializeField] private float cloneDuration;
    [SerializeField] private GameObject clonePrefabs;
    [Space]
    [SerializeField] private bool canAttack;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool canCreateCloneOnCounterAttack;
    [Header("Clone duplication")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    [Header("Crystal instead of clone")]
    public bool createCrystalInsteadOfClone;
    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (createCrystalInsteadOfClone)
        {
            SkillManager.instance.crystal_Skill.CreateCrystal();
            SkillManager.instance.crystal_Skill.CurrentCrystalChooseRandomTarget();
            return;
        }

        GameObject newClone = Instantiate(clonePrefabs);
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, canDuplicateClone, chanceToDuplicate, FindClosestAttack(newClone.transform, 2.25f));
    }
    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }
    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }
    public void CreateCloneOnCounterAttack(Transform _clonePosition, Vector3 _offset)
    {
        if (canCreateCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(_clonePosition, _offset));
        }
    }
    private IEnumerator CreateCloneWithDelay(Transform _clonePosition, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(_clonePosition, _offset);

    }
}
