using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Metalic,
    Glass,
    Water
}

public class HauntedObject : MonoBehaviour
{
    public ObjectType type;
    public Room room;
    public Ghost ghost;

    private AudioSource audio = null;

    private bool isPossessed = false;

    private bool possessionPaused = false;

    void Start()
    {
        EventManager.OnPossessionStart += OnPossessionStart;
        EventManager.OnPossessionStop += OnPossessionStop;
        EventManager.OnPossessionPause += OnPossessionPause;
        EventManager.OnPossessionContinue += OnPossessionContinue;
        EventManager.OnPossessionComplete += OnPossessionComplete;
        audio = GetComponent<AudioSource>();

        if(ghost)
        {
            ghost.AddObject(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnPossessionStart(Room posRoom, ObjectType type)
    {
        if (room == posRoom)
        {
            isPossessed = false;
            audio.Play();
        }
    }

    private void OnPossessionStop(Room posRoom, ObjectType type)
    {
        if (room == posRoom)
        {
            isPossessed = false;
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
        if (room == posRoom)
        {
            audio.Play();
            possessionPaused = false;
        }
    }

    private void OnPossessionComplete(Room posRoom, ObjectType type)
    {
        if (room == posRoom)
        {
            audio.Stop();
            isPossessed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ghost")
        {
            EventManager.StartPossession(room,type);
        }
    }
}
