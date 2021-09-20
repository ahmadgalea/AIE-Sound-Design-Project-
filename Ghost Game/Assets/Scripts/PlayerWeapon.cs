using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    public Image highlightedCursor = null;
    public Image dehighlightedCursor = null;
    public float castDistance = 3;

    private HauntedObject highlightedObject = null;
    private Switch highlighedSwitch = null;

    void Start()
    {
        EventManager.OnShoot += OnShoot;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitObject;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitObject, castDistance))
        {
            Switch lightSwitch = hitObject.transform.gameObject.GetComponent<Switch>();
            if(lightSwitch)
            {
                highlighedSwitch = lightSwitch;
                highlightedCursor.enabled = true;
                dehighlightedCursor.enabled = false;
                return;
            }

            HauntedObject hauntedObject = hitObject.transform.gameObject.GetComponentInParent<HauntedObject>();
            if (hauntedObject && hauntedObject != highlightedObject)
            {
                highlightedObject = hauntedObject;
                highlightedCursor.enabled = true;
                dehighlightedCursor.enabled = false;
                return;
            }
        }
        highlightedCursor.enabled = false;
        dehighlightedCursor.enabled = true;
        highlightedObject = null;
        highlighedSwitch = null;
    }

    private void OnShoot()
    {
        if(highlightedObject)
        {
            EventManager.StopPossession(highlightedObject.room, highlightedObject.type);
        }

        if (highlighedSwitch)
        {
            EventManager.TurnOnLight(highlighedSwitch.room);
        }
    }
}
