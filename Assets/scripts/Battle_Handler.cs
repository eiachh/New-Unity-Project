using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class Battle_Handler : MonoBehaviour
{
    public enum BattleWinner { Friendly,Enemy,Tie};

    //         scene               questid
    List<Tuple<string, BattleWinner, string>> eventsFireAtLoad = new List<Tuple<string, BattleWinner, string>>();

    public class DamageRecievedEventArgs : EventArgs
    {
        public int RecievedDamage { get; set; }
        public bool Cancel { get; set; }
        public DamageRecievedEventArgs(int rec=0, bool cancel=false)
        {
            RecievedDamage = rec;
            Cancel = cancel;
        }
    }

    public class BattleFinishedEventArgs : EventArgs
    {
        public BattleWinner WinnerTeam { get; }
        public string questID { get;}
        public BattleFinishedEventArgs(BattleWinner bw,string qID)
        {
            WinnerTeam = bw;
            questID = qID;
        }
    }

    public delegate void BattleStartedEventHandler(object sender, EventArgs e);
    public event BattleStartedEventHandler BattleStarted;

    public delegate void BattleFinishedEventHandler(object sender, EventArgs e);
    public event EventHandler<BattleFinishedEventArgs> BattleFinished;

    public delegate void DamageRecievedEventHandler(object sender, EventArgs e);
    public event EventHandler<DamageRecievedEventArgs> DamageRecieved;

    public Canvas BattleUI;
    public Text HpDisplayingText;

    List<User_Battle_Unit> friendlies;
    List<Enemy_Base> enemies;

    Battle_Capability_Handler activeCharacter = null;
    Loading_Screen loading_Screen;

    string questID;
    string OriginalScene_ToGoBackAtFinish;
    int speedCap = 100;
    //counts back till the next turn can start it is happening so the user can see the hud changes turnEnding() uses it
    float countBackForNextTurnStart = -1;

    void Start()
    {
        loading_Screen = FindObjectOfType<Loading_Screen>();
    }

    //its retarded... BUT IT WORKS
    void Update()
    {
        if (countBackForNextTurnStart > 0)
        {
            countBackForNextTurnStart -= Time.deltaTime;
        }
        //FIX THIS REEEE
        if (countBackForNextTurnStart <= 0 && countBackForNextTurnStart!=-1)
        {
            countBackForNextTurnStart = -1;
            nextTurn();
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    //Battle without Loading BattleScene
   public void initiateBattleAtLocation(List<User_Battle_Unit> _friendlies,List<Enemy_Base> _enemies,string _questID)
    {
        friendlies = _friendlies;
        enemies = _enemies;

        questID = _questID;

        BattleUI.enabled = true;

        nextTurn();
        BattleStarted(this, EventArgs.Empty);
    }

    //Battle at custom BattleScene
    public void initiateBattleAtPremadeArena(List<User_Battle_Unit> _friendlies, List<Enemy_Base> _enemies,string _questID,string _originalScene)
    {

        loading_Screen.teleportTo("testscene");
       // SceneManager.LoadScene("testscene");

        friendlies = _friendlies;
        enemies = _enemies;

        questID = _questID;
        OriginalScene_ToGoBackAtFinish = _originalScene;

        BattleUI.enabled = true;

        nextTurn();
        
        BattleStarted(this, EventArgs.Empty);
    }

    //handles next turn of the battle
    private void nextTurn()
    {
        
        activeCharacter = calculateNextCharactersTurn();
        if (activeCharacter != null)
        {
            activeCharacter.currentSpeed = 0;
        }
        if (activeCharacter.isAlive)
        {
            if (activeCharacter.typeOfBattler == "player")
            {
                refreshIndicatorBars();
            }
        }
    }

    //by the time, increases the speed. When a character reaches the cap its their turn
    private Battle_Capability_Handler calculateNextCharactersTurn()
    {
        Battle_Capability_Handler temp=null;

        while (temp == null)
        {
            
            temp = checkIfSomebodyHasFull();
            if (temp != null)
            {
                return temp;
            }
            foreach (var item in friendlies)
            {
                if (item.isAlive)
                {
                    item.currentSpeed += item.baseSpeed + item.speedModifier;
                }
            }
            foreach (var item in enemies)
            {
                if (item.isAlive)
                {
                    item.currentSpeed += item.baseSpeed + item.speedModifier;
                }
            }
        }
        return null;
    }

    private Battle_Capability_Handler checkIfSomebodyHasFull()
    {
        var retVal = (Battle_Capability_Handler) friendlies.Find(x => x.currentSpeed >= 100);
        if (retVal != null)
        {
            return retVal;
        }
        retVal = (Battle_Capability_Handler)enemies.Find(x => x.currentSpeed >= 100);
        if (retVal !=null)
        {
            return retVal;
        }
        else
        {
            return null;
        }
    }


    public void useSkill()
    {

    }

    public void Button2_Clicked()
    {
        RecieveDamage_ToCurrentlyActivePartyMember(3);
        turnEnding();
    }

    //returns If player died to the dmg recieved
    public bool RecieveDamage_ToCurrentlyActivePartyMember(int amount)
    {
        DamageRecievedEventArgs e = new DamageRecievedEventArgs();
        if (DamageRecieved != null)
        {
            DamageRecieved(this, e);
        }
        if (!e.Cancel)
        {
            activeCharacter.CurrentHealth = activeCharacter.CurrentHealth - amount;
        }
        
        if (activeCharacter.CurrentHealth <= 0)
        {
            activeCharacter.isAlive = false;
            return false;
        }
        refreshIndicatorBars();
        return true;
    }

    private void refreshIndicatorBars()
    {
        HpDisplayingText.text = activeCharacter.CurrentHealth.ToString();
    }

    private void turnEnding()
    {
        refreshIndicatorBars();
        countBackForNextTurnStart = 1.5f;
    }

    //temporary end fight event
    public void battleUIButtonClicked()
    {
        BattleFinished(this, new BattleFinishedEventArgs(BattleWinner.Friendly, questID));
        BattleUI.enabled = false;
        
        loading_Screen = FindObjectOfType<Loading_Screen>();
        loading_Screen.teleportTo(OriginalScene_ToGoBackAtFinish);
        //BattleFinished(this, new BattleFinishedEventArgs(BattleWinner.Friendly, questID));

    }






    public Image containerParent;
    public Button toAdd;

    //Testing zone

    public void addbuttonto_listview()
    {
        var asd = Instantiate(toAdd);

        asd.transform.parent = containerParent.transform;
    }
    public void moveButtons()
    {
        containerParent.transform.position = new Vector2(containerParent.transform.position.x,containerParent.transform.position.y + 10);
    }
}
