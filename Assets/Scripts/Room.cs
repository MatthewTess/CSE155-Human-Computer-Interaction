using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "My Assets/Room")]
public class Room : ScriptableObject
{
    public string location;
    public string description;
    public string leftChoice, rightChoice; 
    public Room leftRoom, rightRoom;
}
