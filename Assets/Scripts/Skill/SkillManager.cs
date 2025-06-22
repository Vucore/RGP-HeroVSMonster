using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Dash_Skill dash_Skill { get; private set; }
    public Clone_Skill clone_Skill { get; private set; }
    public Sword_Skill sword_Skill { get; private set; }
    public Blackhole_Skill blackhole_Skill { get; private set; }
    public Crystal_Skill crystal_Skill { get; private set; }
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        dash_Skill = GetComponent<Dash_Skill>();
        clone_Skill = GetComponent<Clone_Skill>();
        sword_Skill = GetComponent<Sword_Skill>();
        blackhole_Skill = GetComponent<Blackhole_Skill>();
        crystal_Skill = GetComponent<Crystal_Skill>();
    }
}
