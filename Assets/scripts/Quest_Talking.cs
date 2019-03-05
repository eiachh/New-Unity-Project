using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_Talking : Quest_PartBase
{
    public override void PrepareQuestPart(string questID,bool OnLoadTriggered)
    {
        base.Questtarget.addActiveQuest(questID,visibleMark);
    }
}
