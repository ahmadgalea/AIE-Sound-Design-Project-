using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private HauntedObject highlightedObject = null;
    void Start()
    {
        EventManager.OnHighlightObject += OnHighlightObject;
        EventManager.OnDehighlightObject += OnDehighlightObject;
        EventManager.OnShoot += OnShoot;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnHighlightObject(HauntedObject thisObject)
    {
        highlightedObject = thisObject;
    }

    private void OnDehighlightObject()
    {
        highlightedObject = null;
    }

    private void OnShoot()
    {
        if(highlightedObject)
        {
            EventManager.StopPossession(highlightedObject.room, highlightedObject.type);
        }
    }
}
