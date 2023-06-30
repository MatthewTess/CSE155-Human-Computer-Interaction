using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    Gaze gaze;
    public bool instant = false;
    public float pointerChoice = 0;
    public float deltaMultiplier = 1;
    float xMult = 1;
    float yMult = 1;
    public Vector2 origin = new Vector2(0, 0);
    public Slider x, y;
    public EventSystem eventSystem;
    private GameObject currentObject;

    void Start()
    {
        gaze = FindObjectOfType<Gaze>();
        Voice.onSelect += OnSelect;
    }

    private void OnDestroy()
    {
        Voice.onSelect -= OnSelect;
    }

    public void SetXMult()
    {
        xMult = x.value;
        yMult = y.value;
    }
    public void SetOrigin()
    {
        origin.x = gaze.receivedPos.x;
        origin.y = gaze.receivedPos.y;
    }
    void Update()
    {
        Vector2 centerOfScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        pointerChoice = (transform.position.x - centerOfScreen.x) / (Screen.width / 2);
        Vector2 point = new Vector3(gaze.receivedPos.x, gaze.receivedPos.y);
        point = (xMult) * (((centerOfScreen - origin) + point) - centerOfScreen) + (centerOfScreen - origin) + point;
        point.y = centerOfScreen.y;
        transform.position = !instant ? Vector3.Lerp(transform.position, point, Time.deltaTime * deltaMultiplier) : point;
        if (Input.GetMouseButtonDown(1))
        {
            //origin = Input.mousePosition;
        }

        if (eventSystem.IsPointerOverGameObject())
        {
            currentObject = eventSystem.currentSelectedGameObject;
        }
    }

    public bool IsFixatedOn(GameObject target)
    {
        return Vector2.Distance(transform.position, target.transform.position) < 0.1f;
    }
    public Vector2 GetGazePosition()
    {
        //return transform.position;
        return new Vector2(gaze.receivedPos.x, gaze.receivedPos.y);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Implement the click action here
    }

    private void OnSelect()
    {
        if (currentObject != null)
        {
            ExecuteEvents.Execute(currentObject, null, ExecuteEvents.pointerClickHandler);
        }
    }
}
