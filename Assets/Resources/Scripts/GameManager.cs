using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool GameStarted { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if(!GameStarted && Input.anyKey)
        {
            GameStarted = true;
            EventHandler.TriggerEvent("GameStart");
        }
    }
}
