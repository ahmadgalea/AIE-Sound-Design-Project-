using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostState
{
    Inactive,
    Moving,
    Possessing
}

public class Ghost : MonoBehaviour
{
    public float inactivePeriod = 10.0f;
    public float possessionPeriod = 10.0f;
    public float moveSpeed = 5.0f;

    public GameObject spawnPosition = null;

    private List<HauntedObject> objects;
    private HauntedObject targetObject = null;

    private GhostState state = GhostState.Inactive;

    private float timer = 0.0f;
    private bool possessionPaused = false;

    private AudioSource audio = null;

    void Start()
    {
        EventManager.OnPossessionStart += OnPossessionStart;
        EventManager.OnPossessionStop += OnPossessionStop;
        EventManager.OnPossessionPause += OnPossessionPause;
        EventManager.OnPossessionContinue += OnPossessionContinue;
        EventManager.OnPossessionComplete += OnPossessionComplete;
        EventManager.OnLightOn += OnLightOn;
        EventManager.OnLightOff += OnLightOff;

        audio = GetComponent<AudioSource>();
    }

    public void AddObject(HauntedObject hauntedObject)
    {
        if(objects == null)
        {
            objects = new List<HauntedObject>();
        }
        objects.Add(hauntedObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!possessionPaused)
        {
            timer += Time.deltaTime;

            switch (state)
            {
                case GhostState.Inactive:
                    if (timer >= inactivePeriod)
                    {
                        ChangeState(GhostState.Moving);
                    }
                    break;
                case GhostState.Moving:
                    if (targetObject == null)
                    {
                        TargetObject();
                        audio.Play();
                    }
            
                    UpdatePosition();
                    break;
                case GhostState.Possessing:
                    if (timer >= possessionPeriod)
                    {
                        EventManager.PossessionComplete(targetObject.room, targetObject.type);
                        objects.Remove(targetObject);
                        targetObject = null;
                        ResetPosition();
                        ChangeState(GhostState.Inactive);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void ChangeState(GhostState newState)
    {
        state = newState;
        timer = 0.0f;
    }

    private void TargetObject()
    {
        if (objects.Count > 0)
        {
            int randomInt = Random.Range(0, objects.Count);
            targetObject = objects[randomInt];
        }
        else
        {
            ChangeState(GhostState.Inactive);
        }
    }

    private void UpdatePosition()
    {
        var direction = (targetObject.transform.position - transform.position).normalized;
        transform.position += moveSpeed * Time.deltaTime*direction;
    }

    private void ResetPosition()
    {
        if (spawnPosition)
        {
            transform.position = spawnPosition.transform.position;
        }
    }

    private void OnPossessionStart(Room room, ObjectType type)
    {
        possessionPaused = false;
        audio.Stop();
        ChangeState(GhostState.Possessing);
    }

    private void OnPossessionStop(Room room, ObjectType type)
    {
        possessionPaused = false;
        ResetPosition();
        ChangeState(GhostState.Inactive);
    }

    private void OnPossessionComplete(Room room, ObjectType type)
    {
        possessionPaused = false;
        ResetPosition();
        ChangeState(GhostState.Inactive);
    }

    private void OnPossessionPause(Room room, ObjectType type)
    {
        if (targetObject)
        {
            if (targetObject.room == room)
            {
                possessionPaused = true;
            }
        }
    }

    private void OnPossessionContinue(Room room, ObjectType type)
    {
        if (targetObject)
        {
            if (targetObject.room == room)
            {
                possessionPaused = false;
            }
        }
    }

    private void OnLightOn(Room room)
    {
        if (targetObject && room == targetObject.room)
        {
            EventManager.PausePossession(targetObject.room, targetObject.type);
        }
    }

    private void OnLightOff(Room room)
    {
        if (targetObject && room == targetObject.room)
        {
            EventManager.ContinuePossession(targetObject.room, targetObject.type);
        }
    }
}
