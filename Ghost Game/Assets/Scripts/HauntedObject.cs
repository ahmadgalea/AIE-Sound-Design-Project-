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

    public float possessionTimeLimit = 5.0f;

    private AudioSource audio = null;

    private float possessionTimer = 0.0f;
    private bool ghostPossessing = false;
    private bool isPossessed = false;

    void Start()
    {
        EventManager.OnPossessionStart += OnPossessionStart;
        EventManager.OnPossessionStop += OnPossessionStop;
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
        if(!isPossessed && ghostPossessing)
        {
            possessionTimer += Time.deltaTime;
            if(possessionTimer >= possessionTimeLimit)
            {
                EventManager.PossessionComplete(room, type);
            }
        }
    }
    private void OnPossessionStart(Room room, ObjectType type)
    {
        isPossessed = false;
        ghostPossessing = true;
        possessionTimer = 0.0f;
        audio.Play();
    }

    private void OnPossessionStop(Room room, ObjectType type)
    {
        isPossessed = false;
        ghostPossessing = false;
        possessionTimer = 0.0f;
        audio.Stop();
    }

    private void OnPossessionComplete(Room room, ObjectType type)
    {
        isPossessed = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EventManager.StopPossession(room, type);
        }
        else if(other.tag == "Ghost")
        {
            EventManager.StartPossession(room,type);
        }
    }
}
