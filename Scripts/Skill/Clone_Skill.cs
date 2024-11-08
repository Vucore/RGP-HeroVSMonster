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
    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefabs);
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition , cloneDuration, canAttack, _offset);
    }
}
