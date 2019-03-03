using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Full_Quest : MonoBehaviour
{
    Quest_Handler QH;

    public List<Quest_PartBase> questParts = new List<Quest_PartBase>();
    public int QuestState { get; set; } = 0;

    public bool availableByDefault = false;
    public bool completedQuest = false;
    public string questID;
    public string prerequisiteQuestID="none";

    public Canvas QuestCompletedUI;


    void Awake()
    {
        QH = FindObjectOfType<Quest_Handler>();
        QH.QuestListInitializeFinished += register;
    }
    public void register(object sender, EventArgs e)
    {
        QH.addToQuestList(this, availableByDefault,completedQuest);
    }

    public Npc_Script getTargetNpc()
    {
        return questParts[QuestState].Questtarget;
    }

    public bool getVisibleMark()
    {
        return questParts[QuestState].visibleMark;
    }

    public string getNpcText()
    {
        return questParts[QuestState].NpcQuestText;
    }
    public bool taskCompleted()
    {
        //the bool is never used YET in references
        if (QuestState == questParts.Count-1)
        {
            QuestCompletedUI.enabled = true;
            completedQuest = true;
            return true;
        }
        else
        {
            QuestState++;
            
            questParts[QuestState].PrepareQuestPart(questID);
            return false;
        }
    }
    public void OnLoadSetup(int _state)
    {
        QuestState = _state;
        if (QuestState == questParts.Count - 1)
        {
            QuestCompletedUI.enabled = true;
            completedQuest = true;
            return;
        }
        questParts[QuestState].PrepareQuestPart(questID);
    }
}
