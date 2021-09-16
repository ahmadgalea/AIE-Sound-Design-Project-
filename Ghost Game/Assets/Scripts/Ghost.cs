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

    private AudioSource audio = null;

    void Start()
    {
        EventManager.OnPossessionStart += OnPossessionStart;
        EventManager.OnPossessionStop += OnPossessionStop;
        objects = new List<HauntedObject>();

        audio = GetComponent<AudioSource>();
    }

    public void AddObject(HauntedObject hauntedObject)
    {
        objects.Add(hauntedObject);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        switch(state)
        {
            case GhostState.Inactive:
                if(timer >= inactivePeriod)
                {
                    audio.Play();
                    ChangeState(GhostState.Moving);
                }
                break;
            case GhostState.Moving:
                if (targetObject == null)
                {
                    TargetObject();
                }
                UpdatePosition();
                break;
            case GhostState.Possessing:
                if (timer >= possessionPeriod)
                {
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

    private void ChangeState(GhostState newState)
    {
        state = newState;
        timer = 0.0f;
    }

    private void TargetObject()
    {
        int randomInt = Random.Range(0, objects.Count);
        targetObject = objects[randomInt];
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
        audio.Stop();
        ChangeState(GhostState.Possessing);
    }

    private void OnPossessionStop(Room room, ObjectType type)
    {
        ResetPosition();
        ChangeState(GhostState.Inactive);
    }
}
