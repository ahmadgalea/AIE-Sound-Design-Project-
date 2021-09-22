using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public Material offMaterial = null;
    public Material onMaterial = null;

    public Room room;

    private AudioSource audio = null;


    void Start()
    {
        EventManager.OnLightOn += OnTurnOn;
        EventManager.OnLightOff += OnTurnOff;

        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnOn()
    {
        GetComponent<Renderer>().material = onMaterial;
        audio.Play();
    }

    public void TurnOff()
    {
        GetComponent<Renderer>().material = offMaterial;
        audio.Play();
        //EventManager.TurnOnLight(room);
    }

    private void OnTurnOn(Room eventRoom)
    {
        if (room == eventRoom)
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
