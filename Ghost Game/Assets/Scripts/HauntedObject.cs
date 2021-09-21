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

    public AudioSource possessionAudio = null;
    public AudioSource possessionConcludedAudio = null;
    public AudioSource possessionCancelledAudio = null;

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
            possessionAudio.Play();
        }
    }

    private void OnPossessionStop(Room posRoom, ObjectType posType)
    {
        if (room == posRoom && posType == type)
        {
            state = ObjectState.Saved;
            possessionAudio.Stop();
            possessionCancelledAudio.Play();
        }
    }

    private void OnPossessionPause(Room posRoom, ObjectType type)
    {
        if (room == posRoom)
        {
            possessionAudio.Stop();
            possessionPaused = true;
        }
    }

    private void OnPossessionContinue(Room posRoom, ObjectType type)
    {
        if (state == ObjectState.BeingPossessed && room == posRoom)
        {
            possessionAudio.Play();
            possessionPaused = false;
        }
    }

    private void OnPossessionComplete(Room posRoom, ObjectType posType)
    {
        if (room == posRoom && posType == type)
        {
            possessionAudio.Stop();
            state = ObjectState.Possessed;
            possessionConcludedAudio.Play();
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
