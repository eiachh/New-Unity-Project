using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unique_Party : MonoBehaviour
{
    public List<User_Battle_Unit> ControllablePartyMembers;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
