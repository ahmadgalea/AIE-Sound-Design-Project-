using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject player;

    public Slider hauntingTimer = null;

    private List<HauntedObject> objects = new List<HauntedObject>();
    private HauntedObject targetObject = null;

    private GhostState state = GhostState.Inactive;

    private float timer = 0.0f;
    private bool possessionPaused = false;

    private AudioSource audio = null;
    private MeshRenderer renderer = null;

    void Start()
    {
        EventManager.OnPossessionStart += OnPossessionStart;
        EventManager.OnPossessionStop += OnPossessionStop;
        EventManager.OnPossessionPause += OnPossessionPause;
        EventManager.OnPossessionContinue += OnPossessionContinue;
        EventManager.OnPossessionComplete += OnPossessionComplete;
        EventManager.OnLightOn += OnLightOn;
        EventManager.OnLightOff += OnLightOff;

        EventManager.OnGameStateChanged += OnGameStateChanged;

        audio = GetComponent<AudioSource>();
        renderer = transform.GetComponentInChildren<MeshRenderer>();
        renderer.enabled = false;

        Reset();
    }

    private void Reset()
    {
        objects.AddRange(FindObjectsOfType<HauntedObject>());
        ResetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player.transform.position);
        }
        if (!possessionPaused && objects.Count > 0)
        {
            timer += Time.deltaTime;

            switch (state)
            {
                case GhostState.Inactive:
                    targetObject = null;
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
                    UpdateHauntingSlider(timer);
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

    public HauntedObject GetTarget()
    {
        return targetObject;
    }

    private void ChangeState(GhostState newState)
    {
        state = newState;
        timer = 0.0f;
    }

    private void UpdateHauntingSlider(float timer)
    {
        if(hauntingTimer != null && possessionPeriod > 0)
        {
            hauntingTimer.value = timer / possessionPeriod;
        }

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
        transform.position += moveSpeed* Time.deltaTime * (new Vector3(direction.x,0,direction.z));
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
        if (hauntingTimer != null)
        {
            hauntingTimer.transform.parent.gameObject.SetActive(true);
        }
        possessionPaused = false;
        audio.Stop();
        ChangeState(GhostState.Possessing);
    }

    private void OnPossessionStop(Room room, ObjectType type)
    {
        if (hauntingTimer != null)
        {
            hauntingTimer.transform.parent.gameObject.SetActive(false);
            hauntingTimer.value = 0;
        }
        possessionPaused = false;
        ResetPosition();
        objects.RemoveAll(thisObject => thisObject.GetState() == ObjectState.Saved);
        ChangeState(GhostState.Inactive);
        renderer.enabled = false;
    }

    private void OnPossessionComplete(Room room, ObjectType type)
    {
        if (hauntingTimer != null)
        {
            hauntingTimer.transform.parent.gameObject.SetActive(false);
            hauntingTimer.value = 0;
        }
        possessionPaused = false;
        ResetPosition();
        objects.RemoveAll(thisObject => thisObject.GetState() == ObjectState.Saved);
        ChangeState(GhostState.Inactive);
        renderer.enabled = false;
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
            renderer.enabled = true;
        }
    }

    private void OnLightOff(Room room)
    {
        if (targetObject && room == targetObject.room)
        {
            EventManager.ContinuePossession(targetObject.room, targetObject.type);
            renderer.enabled = false;
        }
    }

    private void OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.Playing)
        {
            Reset();
        }
    }
}
