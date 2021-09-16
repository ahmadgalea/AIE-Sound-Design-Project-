using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EventManager.TurnOnLight(Room.Dining);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EventManager.TurnOnLight(Room.Kitchen);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EventManager.TurnOnLight(Room.Bathroom);
        }
        if(Input.GetMouseButtonDown(0))
        {
            EventManager.Shoot();
        }

        Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitObject;
        if (Physics.Raycast(cursorRay, out hitObject,2))
        {
            HauntedObject hauntedObject = hitObject.transform.gameObject.GetComponentInParent<HauntedObject>();
            if (hauntedObject)
            {
                EventManager.HighlightObject(hauntedObject);
            }
        }
        else
        {
            EventManager.DehighlightObject();
        }
    }
}
