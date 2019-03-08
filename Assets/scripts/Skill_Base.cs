using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill_Base : MonoBehaviour
{
    [Tooltip("The displayed name of the skill.")]
    public string skillName = "Skill";
    [Tooltip("The base damage which can be increased by multipliers of items etc.")]
    public int skillDamage = 0;

    public enum SkillType { Melle, Ranged, Special }
    public enum SkillSelectionType { SingleTarget, MultiTarget, AllEnemy }

    [HideInInspector]
    public SkillType skillType;
    [HideInInspector]
    public SkillSelectionType SelectionType;


    Battle_Handler Battle_H;

    // Start is called before the first frame update
    void Start()
    {
        Battle_H = FindObjectOfType<Battle_Handler>();
    }

    public virtual void performSkill(List<Enemy_Base> enemies)
    {

        //Battle_H.Proceed_TurnEnding();
    }

    public void OnSkillActivated(object sender, EventArgs e)
    {

    }
}