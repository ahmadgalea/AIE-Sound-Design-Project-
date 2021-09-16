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

    private List<HauntedObject> objects;

    private GhostState state = GhostState.Inactive;

    private float timer = 0.0f;

    void Start()
    {
        objects = new List<HauntedObject>();
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
                    ChangeState(GhostState.Moving);
                }
                break;
            case GhostState.Moving:

                break;
            case GhostState.Possessing:

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
}
