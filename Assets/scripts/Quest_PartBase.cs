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
    [Tooltip("The target of talking quest.")]
    public Npc_Script Questtarget;
    [Tooltip("Set this to false if you want to hide the Quest available mark.")]
    public bool visibleMark = true;
    [Tooltip("Text that is shown when you talk to \"Questtarget\" NPC while having the quest ")]
    public string NpcQuestText;
    [Tooltip("Just don't touch it.")]
    public bool IsFinished = false;
    [Tooltip("Set it to false if you don't want the QuestFinish ui to trigger.")]
    public bool CanBeQuestEnd = true;
}
