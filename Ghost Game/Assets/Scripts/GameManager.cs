using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    StartMenu,
    Paused,
    Playing,
    Won,
    Lost,
    Quit
}

public class GameManager : MonoBehaviour
{
    public Text normalObjectsCounter;
    public Text possessedObjectsCounter;
    public Text savedObjectsCounter;

    public GameObject hudScreen = null;
    public GameObject gameWinScreen = null;
    public GameObject gameLossScreen = null;
    public GameObject startScreen = null;

    public AudioSource winAudio = null;
    public AudioSource lossAudio = null;

    private HauntedObject[] objects;

    int normalObjects = 0;
    int possessedObjects = 0;
    int savedObjects = 0;
    int totalObjects = 0;

    public static GameState state = GameState.StartMenu;


    // Start is called before the first frame update
    void Start()
    {
        Reset();

        EventManager.OnGameStateChanged += OnGameStateChanged;

        EventManager.OnPossessionStart += OnPossessionStart;
        EventManager.OnPossessionStop += OnPossessionStop;
        EventManager.OnPossessionPause += OnPossessionPause;
        EventManager.OnPossessionContinue += OnPossessionContinue;
        EventManager.OnPossessionComplete += OnPossessionComplete;

        EventManager.ChangeGameState(GameState.StartMenu);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Reset()
    {
        objects = FindObjectsOfType<HauntedObject>();
        totalObjects = objects.Length;
        normalObjects = totalObjects;
        possessedObjects = 0;
        savedObjects = 0;
        UpdateUI();
    }

    private void OnGameStateChanged(GameState newState)
    {
        state = newState;
        switch(newState)
        {
            case GameState.Playing:
                Reset();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                if (hudScreen != null)
                {
                    hudScreen.SetActive(true);
                }
                if (gameLossScreen != null)
                {
                    gameLossScreen.SetActive(false);
                }
                if (gameWinScreen != null)
                {
                    gameWinScreen.SetActive(false);
                }
                if (startScreen != null)
                {
                    startScreen.SetActive(false);
                }
                break;
            case GameState.Won:

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                if (hudScreen != null)
                {
                    hudScreen.SetActive(false);
                }
                if (gameLossScreen != null)
                {
                    gameLossScreen.SetActive(false);
                }
                if (gameWinScreen != null)
                {
                    gameWinScreen.SetActive(true);
                }
                if (startScreen != null)
                {
                    startScreen.SetActive(false);
                }
                break;
            case GameState.Lost:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                if (hudScreen != null)
                {
                    hudScreen.SetActive(false);
                }
                if (gameLossScreen != null)
                {
                    gameLossScreen.SetActive(true);
                }
                if (gameWinScreen != null)
                {
                    gameWinScreen.SetActive(false);
                }
                if (startScreen != null)
                {
                    startScreen.SetActive(false);
                }
                break;
            case GameState.StartMenu:

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                if (hudScreen != null)
                {
                    hudScreen.SetActive(false);
                }
                if (gameLossScreen != null)
                {
                    gameLossScreen.SetActive(false);
                }
                if (gameWinScreen != null)
                {
                    gameWinScreen.SetActive(false);
                }
                if (startScreen != null)
                {
                    startScreen.SetActive(true);
                }
                break;
            case GameState.Quit:
                Application.Quit();
                break;

        }
    }
    private void UpdateUI()
    {
        normalObjectsCounter.text = "Objects Remaining: " + normalObjects;
        possessedObjectsCounter.text = "Possessed Objects: " + possessedObjects;
        savedObjectsCounter.text = "Saved Objects: " + savedObjects;
    }

    private void OnPossessionStart(Room room, ObjectType type)
    {

    }

    private void OnPossessionStop(Room room, ObjectType type)
    {
        normalObjects--;
        savedObjects++;
        UpdateUI();
        if(savedObjects > (totalObjects/2))
        {
            if(winAudio != null)
            {
                winAudio.Play();
            }
            EventManager.ChangeGameState(GameState.Won);
        }
    }

    private void OnPossessionComplete(Room room, ObjectType type)
    {
        normalObjects--;
        possessedObjects++;
        UpdateUI();
        if (possessedObjects > (totalObjects/2))
        {
            if(lossAudio!=null)
            {
                lossAudio.Play();
            }
            EventManager.ChangeGameState(GameState.Lost);
        }
    }

    private void OnPossessionPause(Room room, ObjectType type)
    {

    }

    private void OnPossessionContinue(Room room, ObjectType type)
    {

    }
}
