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

    private HauntedObject[] objects;

    // Start is called before the first frame update
    void Start()
    {
        //objects = FindObjectOfType(HauntedObject);
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
