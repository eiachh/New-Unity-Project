using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Full_Quest : MonoBehaviour
{
    Quest_Handler QH;

    public List<Quest_PartBase> questParts = new List<Quest_PartBase>();
    int questState = 0;

    public bool availableByDefault = false;
    public bool completedQuest = false;
    public string questID;

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
        return questParts[questState].Questtarget;
    }

    public bool getVisibleMark()
    {
        return questParts[questState].visibleMark;
    }

    public string getNpcText()
    {
        return questParts[questState].NpcQuestText;
    }
    public bool taskCompleted()
    {
        if (questState == questParts.Count-1)
        {
            QuestCompletedUI.enabled = true;
            completedQuest = true;
            return true;
        }
        else
        {
            questState++;
            questParts[questState].PrepareQuestPart(questID);
            return false;
        }
    }
}
