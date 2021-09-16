using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void LightAction(Room room);

    public static event LightAction OnLightOn;
    public static event LightAction OnLightOff;

    // rooms: 0-kitchen, 1-dining, 2-bathroom
    public static void TurnOnLight(int room)
    {
        EventManager.TurnOnLight((Room)room);
    }

    public static void TurnOffLight(int room)
    {
        EventManager.TurnOffLight((Room)room);
    }

    public static void TurnOnLight(Room room)
    {
        if(EventManager.OnLightOn!=null)
        {
            EventManager.OnLightOn(room);
        }
    }

    public static void TurnOffLight(Room room)
    {
        if (EventManager.OnLightOff != null)
        {
            EventManager.OnLightOff(room);
        }
    }

}
