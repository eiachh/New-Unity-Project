using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SingleTarget : Skill_Base
{
    [HideInInspector]
    public SkillSelectionType SelectionType { get; } = SkillSelectionType.SingleTarget;
    [HideInInspector]
    public SkillType skillType { get; } = SkillType.Melle;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void performSkill(List<Enemy_Base> enemies)
    {
        
    }


}
