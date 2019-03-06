using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Quest_PartBase : MonoBehaviour
{
    public virtual void PrepareQuestPart(string questID,bool OnLoadTriggered)
    {
        Debug.Log("This should be overwritten every time");
    }
    public Npc_Script Questtarget;
    public bool visibleMark = true;
    public string NpcQuestText;
    public bool IsFinished = false;
}
