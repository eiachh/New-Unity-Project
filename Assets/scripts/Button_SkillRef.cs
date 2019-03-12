using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_SkillRef : MonoBehaviour
{

    public Skill_Base SkillAttached { get; set; }
    public Button_SkillRef(Skill_Base skill)
    {
        SkillAttached = skill;
    }
}
