using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Unstarted = 0,
    InProgress = 1,
    PlayerDead = 2,
    MissionSuccess = 3
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentGameState = GameState.Unstarted;
    public float ZombieKillsRequiredToProgress = 3;
    public string NextScene;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError(GetType() + " already attached to " + gameObject.name + "!");
        else Instance = this;
    }

    private void Start()
    {
        EventHandler.Subscribe("PlayerDeath", OnPlayerDeath);
        EventHandler.Subscribe("ZombieKilled", OnZombieKilled);
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentGameState == GameState.Unstarted && Input.anyKey)
        {
            CurrentGameState = GameState.InProgress;
            EventHandler.TriggerEvent("GameStart");
        }
        else if(CurrentGameState == GameState.PlayerDead && Input.anyKey && Time.time - deathTime >= 1)
        {
            EventHandler.ClearEventListeners();
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        else if(CurrentGameState == GameState.MissionSuccess && Input.anyKey && Time.time - successTime >= 1)
        {
            if (NextScene != null && NextScene != "")
            {
                EventHandler.ClearEventListeners();
                UnityEngine.SceneManagement.SceneManager.LoadScene(NextScene);
            }
        }
    }

    private float deathTime;
    void OnPlayerDeath(object foo)
    {
        CurrentGameState = GameState.PlayerDead;
        deathTime = Time.time;
    }

    private float successTime;
    void OnZombieKilled(object foo)
    {
        ZombieKillsRequiredToProgress--;
        if (ZombieKillsRequiredToProgress == 0)
        {
            successTime = Time.time;
            CurrentGameState = GameState.MissionSuccess;
            EventHandler.TriggerEvent("MissionSuccess");
        }
    }
}
