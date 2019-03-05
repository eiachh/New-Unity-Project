using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testtype : MonoBehaviour
{
    float asd;
    public UnityEngine.Object scriptFile;
    void Start()
    {
       /* GameObject obj = new GameObject();
        Debug.Log(obj.GetType() == typeof(GameObject));
        Debug.Log(obj.GetType().ToString());
        Debug.Log(typeof(GameObject).ToString());*/
    }

    void Update()
    {
        asd += Time.deltaTime;
        if (asd>3)
        {
            get_type();
        }
    }

    
    public System.Type ScriptType
    {
        get { return scriptFile.GetType(); }
    }

    public void get_type()
    {
        Debug.Log(ScriptType);
        Debug.Log(scriptFile.GetType());
        Debug.Log(this.GetType());
        Debug.Log(scriptFile.GetType());
        Debug.Log(scriptFile is Npc_Script ? "yes" : "no");

        Type type = scriptFile.GetType().UnderlyingSystemType;
        String className = type.Name;
        String nameSpace = type.Namespace;
        
        Debug.Log("Class "+className +" Namespace: "+ nameSpace);

        Npc_Script kek = (Npc_Script)scriptFile;


        //Type type2 = new Type(Npc_Script);
        Type test = Type.GetType("UnityEngine.Npc_Script");
        Debug.Log(test.GetType());
        

        //ScriptType asd;
    }
}
