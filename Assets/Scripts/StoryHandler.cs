using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryHandler : MonoBehaviour
{
    public Room startingRoom;
    public Choice[] choices;
    public TextMeshProUGUI locationText, descriptionText;
    void Start()
    {
        Choice.onChoiceMade += OnChoiceMade;
        ChangeRoom(startingRoom);
    }

    // Update is called once per frame
    void OnChoiceMade(Choice choice)
    {
        ChangeRoom(choice.room);
    }
    void ChangeRoom(Room room)
    {
        locationText.text = room.location;
        descriptionText.text = room.description;
        choices[0].SetText(room.leftChoice);
        choices[1].SetText(room.rightChoice);
        choices[0].SetRoom(room.leftRoom);
        choices[1].SetRoom(room.rightRoom);
    }
}
