using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPositionHolder : MonoBehaviour
{
    public float x
    {
        get { return this.gameObject.transform.position.x; }
    }
    public float y
    {
        get { return this.gameObject.transform.position.y; }
    }
    public float z
    {
        get { return this.gameObject.transform.position.z; }
    }
}
