using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Menu,
    Paused,
    Playing,
    Won,
    Lost
}

public class GameManager : MonoBehaviour
{
    public RoomLight testLight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("e"))
        {
            testLight.TurnOn();
        }
    }
}
