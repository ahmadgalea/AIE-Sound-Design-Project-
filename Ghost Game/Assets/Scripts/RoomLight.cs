using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Room
{
    Kitchen,
    Dining,
    Bathroom,
    Hallway
}
public class RoomLight : MonoBehaviour
{
    public Room room;
    public float lightDuration = 10.0f;
    public float flashingDuration = 1.0f;
    public float flashPeriod = 0.2f;

    public AudioSource tickTockSlow = null;
    public AudioSource tickTockFast = null;
    public AudioSource flicker = null;

    private bool isOn = false;
    private float timer = 0.0f;
    private float flashTimer = 0.0f;
    private Light light = null;

    private bool isFlickering = false;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnLightOn += OnTurnOn;
        EventManager.OnLightOff += OnTurnOff;

        light = GetComponent<Light>();
        Reset();
    }

    public void Reset()
    {
        if(light)
        {
            light.enabled = false;
        }
        isOn = false;
        timer = 0.0f;
        flashTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(isOn)
        {
            timer += Time.deltaTime;
            if(timer >= (lightDuration-flashingDuration))
            {
                tickTockSlow.loop = false;
                if(!tickTockSlow.isPlaying)
                {
                    if (tickTockSlow)
                    {
                        //tickTockSlow.Stop();
                    }
                    if (!tickTockFast.isPlaying)
                    {
                        tickTockFast.Play();
                    }
                }
                FlashUpdate();
            }
            if(timer >= lightDuration)
            {
                tickTockFast.Stop();
                if(!isFlickering)
                {
                    flicker.Play();
                    isFlickering = true;
                }
                if (isFlickering && !flicker.isPlaying)
                {
                    isFlickering = false;
                    EventManager.TurnOffLight(room);
                }
            }
        }
    }

    private void FlashUpdate()
    {
        flashTimer += Time.deltaTime;
        float newIntensity = Mathf.Abs(Mathf.Cos(2 * Mathf.PI* flashTimer/ flashPeriod));
        if (light)
        {
            light.intensity = newIntensity;
        }
    }

    public void TurnOn()
    {
        isOn = true;
        timer = 0.0f;
        flashTimer = 0.0f;

        if (light)
        {
            light.enabled = true;
        }
        if(tickTockSlow)
        {
            tickTockSlow.loop = true;
            tickTockSlow.Play();
        }
    }

    public void TurnOff()
    {
        isOn = false;
        if (light)
        {
            light.enabled = false;
        }
        if (tickTockSlow)
        {
            tickTockSlow.Stop();
        }
        if (tickTockFast)
        {
            tickTockFast.Stop();
        }
       
    }

    public bool IsOn()
    {
        return isOn;
    }

    private void OnTurnOn(Room eventRoom)
    {
        if(room == eventRoom)
        {
            TurnOn();
        }
    }

    private void OnTurnOff(Room eventRoom)
    {
        if (room == eventRoom)
        {
            TurnOff();
        }
    }

}
