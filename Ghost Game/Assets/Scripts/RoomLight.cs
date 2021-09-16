using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Room
{
    Hallway,
    Kitchen,
    Dining,
    Bathroom
}
public class RoomLight : MonoBehaviour
{
    public Room room;
    public float lightDuration = 10.0f;
    public float flashingDuration = 1.0f;
    public float flashPeriod = 0.2f;

    private bool isOn = false;
    private float timer = 0.0f;
    private float flashTimer = 0.0f;
    private Light light = null;

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
                FlashUpdate();
            }
            if(timer >= lightDuration)
            {
                TurnOff();
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
    }

    public void TurnOff()
    {
        isOn = false;
        if (light)
        {
            light.enabled = false;
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
