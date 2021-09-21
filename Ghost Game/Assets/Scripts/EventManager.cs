using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void LightAction(Room room);

    public static event LightAction OnLightOn;
    public static event LightAction OnLightOff;

    public delegate void PossessionAction(Room room, ObjectType type);

    public static event PossessionAction OnPossessionStart;
    public static event PossessionAction OnPossessionStop;
    public static event PossessionAction OnPossessionPause;
    public static event PossessionAction OnPossessionContinue;
    public static event PossessionAction OnPossessionComplete;

    public delegate void ObjectInteraction(HauntedObject thisObject);

    public static event ObjectInteraction OnHighlightObject;

    public delegate void AnonObjectInteraction();
    public static event AnonObjectInteraction OnDehighlightObject;

    public delegate void GunAction();
    public static event GunAction OnShootStart;
    public static event GunAction OnShootEnd;

    public delegate void GameStateEvent(GameState newState);
    public static event GameStateEvent OnGameStateChanged;

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

    public static void StartPossession(Room room,ObjectType type)
    {
        if (EventManager.OnPossessionStart != null)
        {
            EventManager.OnPossessionStart(room,type);
        }
    }

    public static void StopPossession(Room room, ObjectType type)
    {
        if (EventManager.OnPossessionStop != null)
        {
            EventManager.OnPossessionStop(room,type);
        }
    }

    public static void PausePossession(Room room, ObjectType type)
    {
        if (EventManager.OnPossessionPause != null)
        {
            EventManager.OnPossessionPause(room, type);
        }
    }

    public static void ContinuePossession(Room room, ObjectType type)
    {
        if (EventManager.OnPossessionContinue != null)
        {
            EventManager.OnPossessionContinue(room, type);
        }
    }

    public static void PossessionComplete(Room room, ObjectType type)
    {
        if (EventManager.OnPossessionComplete != null)
        {
            EventManager.OnPossessionComplete(room, type);
        }
    }

    public static void HighlightObject(HauntedObject thisObject)
    {
        if (EventManager.OnHighlightObject != null)
        {
            EventManager.OnHighlightObject(thisObject);
        }
    }

    public static void DehighlightObject()
    {
        if (EventManager.OnDehighlightObject != null)
        {
            EventManager.OnDehighlightObject();
        }
    }

    public static void ShootStart()
    {
        if(EventManager.OnShootStart != null)
        {
            EventManager.OnShootStart();
        }
    }

    public static void ShootEnd()
    {
        if (EventManager.OnShootEnd != null)
        {
            EventManager.OnShootEnd();
        }
    }

    public static void ChangeGameState(int newState)
    {
        ChangeGameState((GameState)newState);
    }

    public static void ChangeGameState(GameState newState)
    {
        if(EventManager.OnGameStateChanged != null)
        {
            EventManager.OnGameStateChanged(newState);
        }
    }

}
