using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Metalic,
    Glass,
    Water
}

public class HauntedObject : MonoBehaviour
{
    public ObjectType type;
    public Room room;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
