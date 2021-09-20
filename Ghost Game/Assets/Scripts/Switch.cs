using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public Material offMaterial = null;
    public Material onMaterial = null;

    public Room room;


    void Start()
    {
        EventManager.OnLightOn += OnTurnOn;
        EventManager.OnLightOff += OnTurnOff;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnOn()
    {
        GetComponent<Renderer>().material = onMaterial;
    }

    public void TurnOff()
    {
        GetComponent<Renderer>().material = offMaterial;
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
