using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Menu,
    Paused,
    Playing,
    Won,
    Lost
}

public class GameManager : MonoBehaviour
{
    public Text normalObjectsCounter;
    public Text possessedObjectsCounter;
    public Text savedObjectsCounter;

    private HauntedObject[] objects;

    int normalObjects = 0;
    int possessedObjects = 0;
    int savedObjects = 0;


    // Start is called before the first frame update
    void Start()
    {
        objects = FindObjectsOfType<HauntedObject>();
        normalObjects = objects.Length;
        possessedObjects = 0;
        savedObjects = 0;

        UpdateUI();

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
                Debug.Log("Win");
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
                Debug.Log("Loss");
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
