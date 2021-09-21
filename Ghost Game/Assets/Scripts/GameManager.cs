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

    private HauntedObject[] objects;

    int normalObjects = 0;
    int possessedObjects = 0;
    int savedObjects = 0;

    private GameState state = GameState.Playing;


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
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Reset()
    {
        objects = FindObjectsOfType<HauntedObject>();
        normalObjects = objects.Length;
        possessedObjects = 0;
        savedObjects = 0;
        UpdateUI();
    }

    private void OnGameStateChanged(GameState newState)
    {
        switch(newState)
        {
            case GameState.Playing:
                Reset();
                if(hudScreen != null)
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
                break;
            case GameState.Won:
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
                break;
            case GameState.Lost:
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
        if(normalObjects == 0)
        {
            if(savedObjects > possessedObjects)
            {
                EventManager.ChangeGameState(GameState.Won);
            }
        }
    }

    private void OnPossessionComplete(Room room, ObjectType type)
    {
        normalObjects--;
        possessedObjects++;
        UpdateUI();
        if (normalObjects == 0)
        {
            if (savedObjects <= possessedObjects)
            {
                EventManager.ChangeGameState(GameState.Lost);
            }
        }
    }

    private void OnPossessionPause(Room room, ObjectType type)
    {

    }

    private void OnPossessionContinue(Room room, ObjectType type)
    {

    }
}
