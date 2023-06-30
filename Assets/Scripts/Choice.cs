using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Choice : MonoBehaviour
{
    [SerializeField] private float thresholdValue = 0.5f;
    [SerializeField] private float timeThreshold = 10f;
    [SerializeField] private bool isRightChoice;
    public Color normalColor, selectedColor;
    public Color selectedGradientColor;
    public Color normalGradientColor;
    TextMeshProUGUI text;
    Image SelectionGradient;
    private Pointer pointer;
    public Room room;

    private bool isTimerRunning;
    private float timer;

    public static event Action<Choice> onChoiceMade;
    public void OnSelectWord()
    {
        if (isTimerRunning)
        {
            onChoiceMade?.Invoke(this);
        }
    }

    private void Awake()
    {
        Voice.onSelect += OnSelectWord;
        SelectionGradient = GetComponentInChildren<Image>();
        selectedGradientColor = SelectionGradient.color;
        normalGradientColor = SelectionGradient.color;
        normalGradientColor.a = .1f;
        SelectionGradient.color = normalGradientColor;
        text = GetComponent<TextMeshProUGUI>();
        pointer = FindObjectOfType<Pointer>();
    }

    private void Update()
    {
        bool isHovering = false;
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 centerOfScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            float mouseChoice = (Input.mousePosition.x - centerOfScreen.x) / (Screen.width / 2);
            isHovering = CheckHovering(mouseChoice);
            if (isHovering)
            {
                onChoiceMade?.Invoke(this);
            }
        }
        else
        {
            isHovering = CheckHovering(pointer.pointerChoice);
        }
        // Check if the pointer is hovering over the choice


        if (isHovering)
        {
            RunTimer();
        }
        else
        {
            StopTimer();
        }

    }
    private bool CheckHovering(float choiceValue)
    {
        bool left = choiceValue < thresholdValue && choiceValue > -.95;
        bool right = choiceValue > thresholdValue && choiceValue < .95;
        bool isHovering = isRightChoice ? right : left;
        return isHovering;
    }

    private void RunTimer()
    {
        isTimerRunning = true;
        timer += Time.deltaTime;
        timer = Mathf.Min(timer, timeThreshold);
        text.color = Color.Lerp(normalColor, selectedColor, Mathf.Min(timer / timeThreshold, 1));
        SelectionGradient.color = Color.Lerp(normalGradientColor, selectedGradientColor, Mathf.Min(timer / timeThreshold, 1));
    }

    private void StopTimer()
    {
        isTimerRunning = false;
        timer = 0f;
        text.color = normalColor;
        SelectionGradient.color = normalGradientColor;
    }
    public void SetText(string astext)
    {
        Debug.Log("Setting text to " + astext);
        text.text = astext;
    }
    public void SetRoom(Room asroom)
    {
        Debug.Log("Setting room to " + asroom.location);
        room = asroom;
    }
}