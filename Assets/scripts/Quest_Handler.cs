using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


public class Quest_Handler : MonoBehaviour
{
    //Contains all the registered quests that the CURRENT SCENE contains
    List<Full_Quest> allQuestList = new List<Full_Quest>();


    //Tuple contains questID, QuestState  NEEDED BECAUSE ON SCENE LOAD THE REFERENCE IS BEING DELETED BUT CANT LET THEM NOT BE DELETED
    List<Tuple<string,int>> activeQuestList = new List<Tuple<string,int>>();
    //Tuple contains questID, QuestState  NEEDED BECAUSE ON SCENE LOAD THE REFERENCE IS BEING DELETED BUT CANT LET THEM NOT BE DELETED
    List<Tuple<string,int>> availableQuestList = new List<Tuple<string,int>>();

    //contains the questID's that are completed
    List<string> completedQuestList = new List<string>();

    //Basically was designed to make quests register after initialization NOW it is used to tell quests to register at different states
    public delegate void QuestListInitializeFinishedEventHandler(object sender, EventArgs args);
    public event QuestListInitializeFinishedEventHandler OnFullQuestRegisterRequired;

    [Tooltip("Needs the UI element that has the CanvasGroupComponent <-- That can fade.")]
    public CanvasGroup QuestCompletedUI;
    [Tooltip("The full UI Canvas so it can be disabled after fading.")]
    public Canvas QuestCompletedCanvas;

    UI_Fade fader;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        Battle_Handler Battle_H = FindObjectOfType<Battle_Handler>();
        fader = FindObjectOfType<UI_Fade>();
        Battle_H.BattleFinished += OnBattlefinished;
       
        //At this state telling FullQuest-s to register themselves
        if (OnFullQuestRegisterRequired != null)
        {
            OnFullQuestRegisterRequired(this, EventArgs.Empty);
            handleAvailableQuests();
        }
    }

    //After loading a screen has to register the Quests of that scene and clean up the null junks from the lists
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cleanUpLists();
        if (OnFullQuestRegisterRequired != null)
        {
            
            OnFullQuestRegisterRequired(this, EventArgs.Empty);
            handleAvailableQuests();
        }
    }

    /*Triggers when a battle has finishes determines the winner and if its the Friendly party calls the quest that requires it 

        NOT NECESSARY HAVE ANY QUEST THAT NEEDS THE BATTLE RESULT

   */
    void OnBattlefinished(object sender,Battle_Handler.BattleFinishedEventArgs e)
    {
        Debug.Log("yep bbattle has finished and the winner is: "+e.WinnerTeam);
    }

    private void cleanUpLists()
    {
        allQuestList.RemoveAll(x => x==null);
        availableQuestList.Clear();
    }

    //after initializing the lists the OnFullQuestRegisterRequired event fires and tells all the single_quests to register themselfes this lets you create quests from editor
    public void addToQuestList(Full_Quest x, bool defaultAvailable, bool completed)
    {
        //THIS CHECKS NEEDED FOR THE OnLoadScene() event so the quest wont register themselves multiple times

        //OUTDATED block of comment but kinda correct
        /*if allquestlist not contains this quest => the quest is new and has to be handled
                      if its available by default =>has to be added to availablequestList no other statement needed
       if the prerequisite is none and not default=>its a quest that will be locally activated by another quest
       if the prerequisite is NOT none and the prerequisit quest is completed before ===> it is valid to be an Available quest */
       //OUTDATED

        //if the registering quest is not null Happens if Scene change happens multiple times
        if (x != null)
        {
            //check so the quest won't be stored multiple times
            if (!allQuestList.Contains(x))
            {
                allQuestList.Add(x);
                if (!completed)
                {
                    if (defaultAvailable)
                    {
                       
                        var findID = activeQuestList.Find(y => y.Item1 == x.questID);
                        //If the player progressed in this quest
                        if (findID!=null)
                        {
                            allQuestList.Find(y => y.questID == x.questID).OnLoadSetup(findID.Item2);
                        }
                        //on testing for the case if the starting quest part doesnt have a talking npc
                        else if (x.prerequisiteQuestID=="special")
                        {
                            activeQuestList.Add(new Tuple<string, int>(x.questID, x.QuestState));
                        }
                        //adds to the available quest since it was a default available quest nothing else to check
                        else if (!completedQuestList.Contains(x.questID))
                        {
                            availableQuestList.Add(new Tuple<string, int>(x.questID, x.QuestState));
                        }
                    }
                    //if the prerequisiteQuestID =="none" and its not DefaultAvailable then its waiting for code activation
                    else if (x.prerequisiteQuestID != "none" && (completedQuestList.Find(y => y == x.prerequisiteQuestID) != null))
                    {
                        
                        //this point is reached if its not a quest starting point BUT the player has a quest that he progressed BUT didnt finish yet
                        if (!completedQuestList.Contains(x.questID))
                        {
                            var findID = activeQuestList.Find(y => y.Item1 == x.questID);
                            if (findID != null )
                            {

                                var temp = allQuestList.Find(y => y.questID == x.questID);
                                temp.OnLoadSetup(findID.Item2);
                            }
                            //if it's a continuationOF a quest then adding for active quest for the exclamationmark symbol
                            else if ( x.ContinuationOfQuest)
                            {
                                activeQuestList.Add(new Tuple<string, int>(x.questID,0));
                                x.OnLoadSetup(0);
                            }
                            else
                            {
                                availableQuestList.Add(new Tuple<string, int>(x.questID, x.QuestState));
                            }
                        }
                    }
                }
                //probably never will enter because quests register at screen load when the default values are loaded THEREFORE
                //   PLACEHOLDER FOR LATER WHEN SAVEGAME WILL BE A THING
                else
                {
                    if (!completedQuestList.Contains(x.questID))
                    {
                        completedQuestList.Add(x.questID);
                    }
                }
            }
        }
    }


    //sets the npc's up who should give the quests (questionable since not all quest should start with an npc talk 
    //
    // AVOIDABLE BY SETTING  prerequisiteQuestID=="special" and ActiveByDefault == True
    //
    private void handleAvailableQuests()
    {
        foreach (var item in availableQuestList)
        {
            var getQuestReference = allQuestList.Find(x => x.questID == item.Item1);
            if (getQuestReference != null)
            {
                getQuestReference.getTargetNpc().addAvailableQuest(getQuestReference.questID, getQuestReference.getVisibleMark());
            }
            else
            {
                Debug.Log("Serious ERROR availableQuestList contains id that is not exceist in allquestList");
            }
        }
    }

    public string getNpcQuestStartingText(string ID)
    {
        var temp = allQuestList.Find(x => x.questID == ID);
        if (temp != null)
        {
            return temp.getNpcText();
        }
        return "";
    }
    //idk why this exceists but it does something? i guess
    public string getNpcQuestEndText(string ID)
    {
        var temp = allQuestList.Find(x => x.questID == ID);
        if (temp != null)
        {
            return temp.getNpcText();
        }
        return "";
    }

    public void questAcceptedWithID(string ID)
    {
        var temp = availableQuestList.Find(x => x.Item1 == ID);
        var questRef = allQuestList.Find(x => x.questID == ID);
        if (temp != null)
        {
            questRef.taskCompleted();
            //state increased since quest got accepted
            Tuple<string, int> questID_state = new Tuple<string, int>(temp.Item1,temp.Item2+1);
            

            activeQuestList.Add(questID_state);
            availableQuestList.Remove(temp);

        }
    }
    public void taskCompletedWithID(string ID)
    {
        var temp = allQuestList.Find(x => x.questID == ID);

        //it looks this retarded since tUpLe Is ReAd oNly I can be bothered fix it if you see it and can be bothered
        var toIncrease = activeQuestList.Find(x => x.Item1 == ID);
        
        Tuple<string, int> questID_state = new Tuple<string, int>(toIncrease.Item1, toIncrease.Item2 + 1);
        activeQuestList.Remove(toIncrease);
        activeQuestList.Add(questID_state);
        if (temp != null)
        {
            temp.taskCompleted();
        }
    }

    public void questCompleted(string qID)
    {

        var temp = activeQuestList.Find(x => x.Item1 == qID);
        activeQuestList.Remove(temp);

        completedQuestList.Add(qID);

        QuestCompletedCanvas.enabled = true;
        QuestCompletedUI.enabled = true;
        QuestCompletedUI.alpha = 1.0f;

        fader.FadeOut(QuestCompletedCanvas,QuestCompletedUI,2);
    }

    public void ReverseStateWithOne(string qID)
    {
        var temp = activeQuestList.Find(x => x.Item1 == qID);
        if (temp.Item2 == 1)
        {
            activeQuestList.Remove(temp);
            availableQuestList.Add(temp);
        }
        else if (temp.Item2==0)
        {
            Debug.Log("Error activeQuestlist at quID: " + temp.Item1 + " state is 0 and is reversing with one");
        }
        else
        {
            Tuple<string, int> questID_state = new Tuple<string, int>(temp.Item1, (temp.Item2 - 1));
            activeQuestList.Remove(temp);
            activeQuestList.Add(questID_state);
            var fullquest = allQuestList.Find(x => x.questID == qID);
            fullquest.ReverseStateTo(questID_state.Item2);
        }
    }
}
