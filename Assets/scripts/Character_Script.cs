using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    List<Interractable> interractablesUpClose = new List<Interractable>();

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
    private int decideFromInterractionList()
    {
        int ret = -1;

        int ind = 0;
        float min = 90000.0f;
        foreach (var item in interractablesUpClose)
        {
            float temp = Mathf.Abs(item.self.transform.position.x - self.transform.position.x) + Mathf.Abs(item.self.transform.position.y - self.transform.position.y);
            if (min > temp)
            {
                min = temp;
                ret = ind;
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
            if (!isInterracting)
            {
                openedInterractionIndex = decideFromInterractionList();
                if (openedInterractionIndex >= 0)
                {
                    isInterracting = interractablesUpClose[openedInterractionIndex].Interract();
                    rigigbody2D.velocity = new Vector2(0, 0);
                }
            }
            else
            {
                isInterracting = interractablesUpClose[openedInterractionIndex].nextDialog();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInterracting)
            {
                interractablesUpClose[openedInterractionIndex].interruptMessage();
            }
            isInterracting = false;
        }
    }

    void FixedUpdate()
    {
       
        if (!isInterracting)
        {
            float moveH = Input.GetAxis("Horizontal");
            float moveV = Input.GetAxis("Vertical");
            rigigbody2D.velocity = new Vector2(moveH * 5, moveV * 5);
        }
    }
}
