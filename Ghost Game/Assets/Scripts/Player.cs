using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private HauntedObject highlightedObject = null;
    void Start()
    {
        EventManager.OnShoot += OnShoot;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitObject;
        if (Physics.Raycast(transform.position, transform.forward, out hitObject, 2))
        {
            HauntedObject hauntedObject = hitObject.transform.gameObject.GetComponentInParent<HauntedObject>();
            if (hauntedObject && hauntedObject != highlightedObject)
            {
                highlightedObject = hauntedObject;
                return;
            }
        }
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
