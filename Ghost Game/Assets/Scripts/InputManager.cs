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
        if (GameManager.state == GameState.Playing)
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
            if (Input.GetMouseButtonDown(0))
            {
                EventManager.ShootStart();
            }
            if (Input.GetMouseButtonUp(0))
            {
                EventManager.ShootEnd();
            }
        }
    }
}
