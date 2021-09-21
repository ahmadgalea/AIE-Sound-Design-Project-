using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    public Image highlightedCursor = null;
    public Image dehighlightedCursor = null;
    public Slider shootTimeGuage = null; 
    public float castDistance = 3;
    public float totalShootTime = 2;

    private Ghost highlightedObject = null;
    private Switch highlighedSwitch = null;

    private bool isShooting = false;
    private float shootTimer = 0.0f;

    void Start()
    {
        EventManager.OnShootStart += OnShootStart;
        EventManager.OnShootEnd += OnShootEnd;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateShoot();
        UpdateRaycast();
    }

    private void UpdateShoot()
    {
        // update shoot
        if (isShooting)
        {
            shootTimer += Time.deltaTime;
            if (shootTimeGuage)
            {
                shootTimeGuage.value = shootTimer / totalShootTime;
            }
            if (shootTimer >= totalShootTime)
            {
                if (highlightedObject)
                {
                    HauntedObject hauntedObject = highlightedObject.GetTarget();
                    EventManager.StopPossession(hauntedObject.room, hauntedObject.type);
                }
                shootTimeGuage.value = 0;
                shootTimeGuage.transform.parent.gameObject.SetActive(false);
                shootTimer = 0.0f;
                isShooting = false;
            }
        }
    }

    private void UpdateRaycast()
    {
        highlightedCursor.enabled = false;
        dehighlightedCursor.enabled = true;
        highlightedObject = null;
        highlighedSwitch = null;

        // update raycasy
        RaycastHit hitObject;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitObject, castDistance))
        {
            Switch lightSwitch = hitObject.transform.gameObject.GetComponent<Switch>();
            if (lightSwitch)
            {
                highlighedSwitch = lightSwitch;
                highlightedCursor.enabled = true;
                dehighlightedCursor.enabled = false;
                return;
            }

            Ghost ghost = hitObject.transform.gameObject.GetComponent<Ghost>();
            if (ghost)
            {
                highlightedObject = ghost;
                highlightedCursor.enabled = true;
                dehighlightedCursor.enabled = false;
                return;
            }
        }
    }

    private void OnShootStart()
    {
        if(highlightedObject)
        {
            shootTimeGuage.value = 0;
            shootTimer = 0.0f;
            shootTimeGuage.transform.parent.gameObject.SetActive(true);
            isShooting = true;
        }

        if (highlighedSwitch)
        {
            EventManager.TurnOnLight(highlighedSwitch.room);
        }
        
    }

    private void OnShootEnd()
    {
        if (highlightedObject)
        {
            if (isShooting)
            {
                shootTimeGuage.value = 0;
                shootTimeGuage.transform.parent.gameObject.SetActive(false);
                isShooting = false;
            }
        }
    }
}
