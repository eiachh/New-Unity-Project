using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public string mapNameToTeleportTo = "";

    private void OnTriggerEnter2D(Collider2D other)
    {
        var loading_Screen = FindObjectOfType<Loading_Screen>();
        loading_Screen.teleportTo(mapNameToTeleportTo);
    }
}
