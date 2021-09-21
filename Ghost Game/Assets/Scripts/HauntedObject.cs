using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Metalic,
    Glass,
    Water
}

public enum ObjectState
{
    Normal,
    BeingPossessed,
    Possessed,
    Saved
}

public class HauntedObject : MonoBehaviour
{
    public ObjectType type;
    public Room room;

    private AudioSource audio = null;

    private ObjectState state = ObjectState.Normal;

    private bool possessionPaused = false;

    void Start()
    {
        EventManager.OnPossessionStart += OnPossessionStart;
        EventManager.OnPossessionStop += OnPossessionStop;
        EventManager.OnPossessionPause += OnPossessionPause;
        EventManager.OnPossessionContinue += OnPossessionContinue;
        EventManager.OnPossessionComplete += OnPossessionComplete;

        EventManager.OnGameStateChanged += OnGameStateChanged;

        audio = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnPossessionStart(Room posRoom, ObjectType posType)
    {
        if (room == posRoom && posType == type)
        {
            state = ObjectState.BeingPossessed;
            audio.Play();
        }
    }

    private void OnPossessionStop(Room posRoom, ObjectType posType)
    {
        if (room == posRoom && posType == type)
        {
            state = ObjectState.Saved;
            audio.Stop();
        }
    }

    private void OnPossessionPause(Room posRoom, ObjectType type)
    {
        if (room == posRoom)
        {
            audio.Stop();
            possessionPaused = true;
        }
    }

    private void OnPossessionContinue(Room posRoom, ObjectType type)
    {
        if (state == ObjectState.BeingPossessed && room == posRoom)
        {
            audio.Play();
            possessionPaused = false;
        }
    }

    private void OnPossessionComplete(Room posRoom, ObjectType posType)
    {
        if (room == posRoom && posType == type)
        {
            audio.Stop();
            state = ObjectState.Possessed;
        }
    }

    public ObjectState GetState()
    {
        return state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ghost" && state == ObjectState.Normal)
        {
            if (other.GetComponent<Ghost>().GetTarget() == this)
            {
                EventManager.StartPossession(room, type);
            }
        }
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            state = ObjectState.Normal;
        }
    }
}
