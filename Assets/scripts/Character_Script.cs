using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character_Script : MonoBehaviour
{

    public GameObject self;
    //public BoxCollider2D collider2Da;
    //public BoxCollider2D collider2D;
    public Rigidbody2D rigigbody2D;
    //public Interractable target;
    //public GameObject target2;
    bool isInterracting = false;
    int openedInterractionIndex;
    Battle_Handler BattleH;
    bool characterInBattle=false;

    List<Interractable> interractablesUpClose = new List<Interractable>();


    void Start()
    {
        BattleH = FindObjectOfType<Battle_Handler>();
        BattleH.BattleStarted += Battle_Has_Started;
        BattleH.BattleFinished += Battle_Has_Finished;
    }
    void Awake()
    {
        var temp = FindObjectOfType<WorldPositionHolder>();
        Vector3 vect = new Vector3(temp.x, temp.y, temp.z);
        gameObject.transform.position = vect;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        interractablesUpClose.Clear();
    }
    /*void LateUpdate()
    {
        Camera temp = FindObjectOfType<Camera>();
        Vector3 distance = new Vector3(0, 0, -100);
        temp.transform.position = this.transform.position+distance;
    }*/

        public void addForInterraction(Interractable x)
    {
        interractablesUpClose.Add(x);
    }
    public void removeForInterraction(Interractable x)
    {
        
        if (interractablesUpClose.Contains(x))
        {
            interractablesUpClose.Remove(x);
        }
    }

    //calculates which interractable object is closer to the character and returns the index of the closer one for further interraction
    private Interractable decideFromInterractionList()
    {
        Interractable ret = null;

        int ind = 0;
        float min = 90000.0f;
        foreach (var item in interractablesUpClose)
        {
            float temp = Mathf.Abs(item.self.transform.position.x - self.transform.position.x) + Mathf.Abs(item.self.transform.position.y - self.transform.position.y);
            if (min > temp)
            {
                min = temp;
                ret = item;
            }
            ind++;
        }
        return ret;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            var item = decideFromInterractionList();
            if (!isInterracting)
            {
                if (item != null)
                {
                    isInterracting = item.Interract();
                    rigigbody2D.velocity = new Vector2(0, 0);
                }
            }
            else
            {
                isInterracting = item.nextDialog();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInterracting)
            {
                interractablesUpClose.Find(x => x.Equals(decideFromInterractionList())).interruptMessage();
            }
            isInterracting = false;
        }
    }

    public void Battle_Has_Started(object sender,EventArgs e)
    {
        characterInBattle = true;
    }
    public void Battle_Has_Finished(object sender, EventArgs e)
    {
        characterInBattle = false;
    }

    void FixedUpdate()
    {
        if (characterInBattle)
        {

        }
        else if (!isInterracting)
        {
            float moveH = Input.GetAxis("Horizontal");
            float moveV = Input.GetAxis("Vertical");
            rigigbody2D.velocity = new Vector2(moveH * 5, moveV * 5);
        }
    }
}
