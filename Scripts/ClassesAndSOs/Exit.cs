using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //This lets a class be seen in the Inspector
public class Exit
{
    public string direction;
    public string description;
    public Room destination;
}
