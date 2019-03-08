using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class Battle_Handler : MonoBehaviour
{
    public enum BattleWinner { Friendly, Enemy, Tie };

    //         scene               questid
    List<Tuple<string, BattleWinner, string>> eventsFireAtLoad = new List<Tuple<string, BattleWinner, string>>();

    public class DamageRecievedEventArgs : EventArgs
    {
        public int RecievedDamage { get; set; }
        public bool Cancel { get; set; }
        public Battle_Capability_Handler DamageSufferer;

        public DamageRecievedEventArgs(int rec = 0, bool cancel = false)
        {
            RecievedDamage = rec;
            Cancel = cancel;
        }
    }

    public class BattleFinishedEventArgs : EventArgs
    {
        public BattleWinner WinnerTeam { get; }
        public string questID { get; }
        public BattleFinishedEventArgs(BattleWinner bw, string qID)
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

    List<User_Battle_Unit> friendlies = new List<User_Battle_Unit>();
    List<Enemy_Base> enemies = new List<Enemy_Base>();

    Battle_Capability_Handler activeCharacter = null;
    Loading_Screen loading_Screen;
    UI_Battle_Controller UI_Control;

    string questID;
    string OriginalScene_ToGoBackAtFinish;
    int speedCap = 100;
    //counts back till the next turn can start it is happening so the user can see the hud changes Proceed_TurnEnding() uses it
    float countBackForNextTurnStart = -1;

    void Start()
    {
        loading_Screen = FindObjectOfType<Loading_Screen>();
        UI_Control = FindObjectOfType<UI_Battle_Controller>();
    }

    //its retarded... BUT IT WORKS    (after turn Proceede_endTurn() countBackForNExtTurnStart set to something > 0 and that causes next turn to happen)
    void Update()
    {
        if (countBackForNextTurnStart > 0)
        {
            countBackForNextTurnStart -= Time.deltaTime;
        }
        //FIX THIS REEEE
        if (countBackForNextTurnStart <= 0 && countBackForNextTurnStart != -1)
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
    public void initiateBattleAtLocation(List<User_Battle_Unit> _friendlies, List<Enemy_Base> _enemies, string _questID)
    {
        BattleUI.enabled = true;
        emptyLists();
        friendlies = _friendlies;
        enemies = _enemies;

        questID = _questID;

        

        nextTurn();
        BattleStarted(this, EventArgs.Empty);
    }

    //Battle at custom BattleScene
    public void initiateBattleAtPremadeArena(List<User_Battle_Unit> _friendlies, List<Enemy_Base> _enemies, string _questID, string _originalScene)
    {
        BattleUI.enabled = true;
        emptyLists();
        loading_Screen.teleportTo("testscene");
        // SceneManager.LoadScene("testscene");

        friendlies = _friendlies;
        enemies = _enemies;

        questID = _questID;
        OriginalScene_ToGoBackAtFinish = _originalScene;

        

        nextTurn();

        BattleStarted(this, EventArgs.Empty);
    }
    private void emptyLists()
    {
        activeCharacter = null;
        enemies.Clear();
        friendlies.Clear();

    }
    //handles next turn of the battle
    private void nextTurn()
    {
        
        activeCharacter = calculateNextCharactersTurn();
        Debug.Log("UpdateUI " + activeCharacter);
        UI_Control.UpdateUIWithBattler(activeCharacter);
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
        Battle_Capability_Handler temp = null;

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
        var retVal = (Battle_Capability_Handler)friendlies.Find(x => x.currentSpeed >= 100);
        if (retVal != null)
        {
            return retVal;
        }
        retVal = (Battle_Capability_Handler)enemies.Find(x => x.currentSpeed >= 100);
        if (retVal != null)
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
        ApplyDamage_ToCurrentlyActivePartyMember(3);
        Proceed_TurnEnding();
    }

    //returns If player died to the dmg recieved
    public bool ApplyDamage_ToCurrentlyActivePartyMember(int damage)
    {
        DamageRecievedEventArgs e = new DamageRecievedEventArgs();
        e.DamageSufferer = activeCharacter;
        if (DamageRecieved != null)
        {
            DamageRecieved(this, e);
        }
        if (!e.Cancel || activeCharacter.isAlive)
        {
            activeCharacter.CurrentHealth = activeCharacter.CurrentHealth - CalcDamage(activeCharacter,damage);
        }
        checkIfStillAlive(activeCharacter);
        
        refreshIndicatorBars();
        return true;
    }

    public void ApplyDamage_ToEnemies(List<Enemy_Base> _enemies,int damage)
    {
        foreach (var sufferer in _enemies)
        {
            DamageRecievedEventArgs e = new DamageRecievedEventArgs();
            e.DamageSufferer = sufferer;
            if (DamageRecieved != null)
            {
                DamageRecieved(this, e);
            }
            if (!e.Cancel || sufferer.isAlive)
            {
                sufferer.CurrentHealth -= CalcDamage(sufferer, damage);
            }
            checkIfStillAlive(activeCharacter);

            refreshIndicatorBars();
        }
    }

    private void refreshIndicatorBars()
    {
        

        UI_Control.UpdateUIBars(activeCharacter.CurrentHealth);

        //placeholder for other updates
    }

    //This should be called by skills
    public void Proceed_TurnEnding()
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

    private int CalcDamage(Battle_Capability_Handler _character, int damage)
    {
        return damage;
    }

    private bool checkIfStillAlive(Battle_Capability_Handler _character)
    {
        if (_character.CurrentHealth>0 && _character.isAlive)
        {
            return true;
        }
        else
        {
            if (_character.isAlive == true)
            {
                _character.isAlive = false;
            }
            return false;
        }
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
