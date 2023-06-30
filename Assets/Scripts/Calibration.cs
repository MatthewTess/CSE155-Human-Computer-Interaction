using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour
{
    public List<Vector2> calibrationPositions;
    public float maxError = 50f;
    public int numTrials = 5;
    public GameObject cursor;
    public Pointer gazeTracker;
    public GameObject fixationTarget;
    private int currentCalibrationPosition = 0;
    private int numCalibrationTrials = 0;
    private float totalError = 0f;
    private float averageError = 0f;
    public Vector2 gazeToCursorScale = Vector2.one;
    public Vector2 gazeToCursorPosition = Vector2.zero;
    void Start()
    {
        cursor.transform.position = new Vector3(calibrationPositions[0].x, calibrationPositions[0].y, cursor.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (gazeTracker.IsFixatedOn(fixationTarget))
        {
            Vector2 gazePosition = gazeTracker.GetGazePosition();
            float error = Vector2.Distance(gazePosition, calibrationPositions[currentCalibrationPosition]);

            totalError += error;
            numCalibrationTrials++;

            if (error <= maxError)
            {
                currentCalibrationPosition++;

                if (currentCalibrationPosition >= calibrationPositions.Count)
                {
                    averageError = totalError / numCalibrationTrials;
                    gazeToCursorScale = new Vector2(Screen.width / averageError, Screen.height / averageError);
                    gazeToCursorPosition = new Vector2(Screen.width / 2f, Screen.height / 2f) - (gazeToCursorScale * calibrationPositions[0]);
                    cursor.transform.position = new Vector3(gazePosition.x * gazeToCursorScale.x + gazeToCursorPosition.x, gazePosition.y * gazeToCursorScale.y + gazeToCursorPosition.y, cursor.transform.position.z);

                    currentCalibrationPosition = 0;
                    numCalibrationTrials = 0;
                    totalError = 0f;
                    averageError = 0f;

                    Debug.Log("Calibration complete");
                }
                else
                {
                    cursor.transform.position = new Vector3(calibrationPositions[currentCalibrationPosition].x, calibrationPositions[currentCalibrationPosition].y, cursor.transform.position.z);
                }
            }
            else
            {
                Debug.Log("Please retry the current calibration position");
            }

            fixationTarget.transform.position = new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), fixationTarget.transform.position.z);
        }
        Vector2 gazePositionCurrent = gazeTracker.GetGazePosition();
        cursor.transform.position = new Vector3(gazePositionCurrent.x * gazeToCursorScale.x + gazeToCursorPosition.x, gazePositionCurrent.y * gazeToCursorScale.y + gazeToCursorPosition.y, cursor.transform.position.z);
    }

}