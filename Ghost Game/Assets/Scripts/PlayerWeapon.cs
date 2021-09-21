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

    private bool aLightIsOn = false;
    private Transform initialTransform;


    void Start()
    {
        EventManager.OnShootStart += OnShootStart;
        EventManager.OnShootEnd += OnShootEnd;
        EventManager.OnLightOff += OnLightOff;

        EventManager.OnGameStateChanged += OnGameStateChanged;
        initialTransform = transform;
    }

    private void Reset()
    {
        transform.position = initialTransform.position;
        transform.rotation = initialTransform.rotation;
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
                if (highlightedObject != null)
                {
                    HauntedObject hauntedObject = highlightedObject.GetTarget();
                    EventManager.StopPossession(hauntedObject.room, hauntedObject.type);
                    EventManager.TurnOffLight(hauntedObject.room);
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

        if (highlighedSwitch && !aLightIsOn)
        {
            EventManager.TurnOnLight(highlighedSwitch.room);
            aLightIsOn = true;
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

    private void OnLightOff(Room room)
    {
        aLightIsOn = false;
    }

    private void OnGameStateChanged(GameState newState)
    {
        var controller = transform.parent.GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        switch (newState)
        {
            case GameState.Playing:
                Reset();
                controller.enabled = true;
                break;
            case GameState.Won:
                controller.enabled = false;
                break;
            case GameState.Lost:
                controller.enabled = false;
                break;
            default:
                break;
        }
    }
}
